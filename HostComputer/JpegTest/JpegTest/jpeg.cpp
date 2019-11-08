# include <string>
# include "jpeg.h"
using namespace std;

BrightnessQuantizedValueTable brightnessQuantizedValueTable;//定义一个亮度量化值表

BrightnessDC_DifferenceTableList brightnessDC_DifferenceTableList;//定义一个亮度DC差值码表

AC_EntropyCoding_MiddleSymbol ac_EntropyCoding_MiddleSymbol[Image_N*Image_N];//由于用函数返回结构体数组，里面的字符串会出现一些无法处理的乱码，故定义为全局变量 

EntropyCoding ac_EntropyCodingStr[Image_N*Image_N];//由于用函数返回结构体数组，里面的字符串会出现一些无法处理的乱码，故定义为全局变量 

StringMapList stringMapList;//定义一个 部分常用亮度AC码表


/*DC差值范围表，本人通过对表找规律发现如下规律 */
int DC_Difference(int temp){
    int temp1;
    if (temp == 0){
        temp1 = 0;
    }
    else{
        for (int i = 1; i <= 11; i++){
            if (abs(temp)<pow(2, i)){
                temp1 = i;
                break;
            }
        }
    }
    return temp1;
}


/*AC系数范围表,本人通过对表找规律发现如下规律*/
int AC_Difference(int temp){
    int temp1;
    if (temp == 0){
        temp1 = 0;
    }
    else{
        for (int i = 1; i <= 10; i++){
            if (abs(temp)<pow(2, i)){
                temp1 = i;
                break;
            }
        }
    }
    return temp1;
}


/*将正整数十进制转换成二进制*/
string TenToTwo(int temp){
    string strTemp = "";
    //旧方法 
    //	for(int k=abs(temp);k>0;k=k/2){
    //		strTemp = strTemp + (k%2==1?'1':'0');
    //	}
    //	//倒置 
    //	int len = strTemp.length();
    //	for(int k=0;k<len/2;k++){
    //		char t = strTemp[k];
    //		strTemp[k] = strTemp[len-1-k];
    //		strTemp[len-1-k] = t;
    //	}
    //新方法 
    char str[Image_N*Image_N];
    _itoa_s(temp, str, 2);
    strTemp = str;
    return strTemp;
}


/*将正整数二进制转换成十进制*/
int TwoToTen(string strTemp){
    int temp = 0;
    for (int i = 0; i<strTemp.length(); i++){
        temp = temp * 2 + strTemp[i] - '0';
    }
    return temp;
}

/*将一个负数的二进制串逐位取反*/
string ConvertToComplement(string strTemp){
    string str = "";
    for (int i = 0; i<strTemp.length(); i++){
        str = str + (strTemp[i] == '1' ? '0' : '1');
    }
    return str;
}

/*DC系数编码*/
EntropyCoding DC_EntropyCoding(int &temp, int &temp1){
    //对DC系数生成中间符号（中间符号(temp1,temp)）

    //查DC差值表
    temp1 = DC_Difference(temp);

    /*测试*/
    //	cout<<temp1<<endl;

    //对中间符号通过查表进行符号编码 
    //对 temp1通过查亮度DC差值码表进行熵编码
    EntropyCoding dc_EntropyCodingStr;
    dc_EntropyCodingStr.strTemp1 = brightnessDC_DifferenceTableList.brightnessDC_DifferenceTable[temp1];

    //对 temp进行转换成补码 
    //先将 temp转换成二进制串
    dc_EntropyCodingStr.strTemp = TenToTwo(abs(temp));
    //转换成补码
    if (temp<0){
        dc_EntropyCodingStr.strTemp = ConvertToComplement(dc_EntropyCodingStr.strTemp);
    }

    /*测试*/
    //	cout<<dc_EntropyCodingStr.strTemp1<<"\t"<<dc_EntropyCodingStr.strTemp<<endl;

    return dc_EntropyCodingStr;
}


/*AC系数编码*/
bool AC_EntropyCoding(int F_[Image_N][Image_N], int &index){
    //对AC系数生成中间符号中/后的部分 
    int SSSS[Image_N][Image_N];

    //查AC系数范围表,本人通过对表找规律发现如下规律 
    for (int i = 0; i<Image_N; i++){
        for (int j = 0; j<Image_N; j++){
            SSSS[i][j] = AC_Difference(F_[i][j]);
        }
    }

    /*测试*/
    //	for(int i=0;i<Image_N;i++){
    //		for(int j=0;j<Image_N;j++){
    //			cout<<SSSS[i][j]<<" ";
    //		}
    //		cout<<endl;
    //	}

    //Z字形编码
    int count = 0;//计算0的个数
    int i, j, t;
    for (i = 0, j = 1, t = 1; t <= Image_N - 2; t++){//以下语句设为一个周期，大概要执行Image_N-2个周期（这里Image_N=8，通过观察发现每一下一上为一周期，则有6个周期+半段） 
        //向左下方向 
        for (; i<Image_N&&j >= 0; i++, j--){
            if (F_[i][j] == 0){
                count++;
            }
            else{
                char countString[Image_N*Image_N];
                _itoa_s(count, countString, 10);//将整数count转换为字符串并保存在countString（以10进制方式，也可指定2、8、10、16等进制实现进制转换，进制转换新玩法） 
                string strTemp = "/";
                strTemp = countString + strTemp;
                //cout<<"--"<<strTemp<<"--"<<endl;
                char SSSS_String[Image_N*Image_N];
                _itoa_s(SSSS[i][j], SSSS_String, 10);
                strTemp = strTemp + SSSS_String;
                //cout<<"**"<<strTemp<<"**"<<endl;

                //中间符号 
                ac_EntropyCoding_MiddleSymbol[index].R_S = strTemp;
                ac_EntropyCoding_MiddleSymbol[index].temp = F_[i][j];
                index++;
                count = 0;//置为0 ，计算下个不为0的数前面0的个数 
            }
        }
        if (i >= Image_N&&j<0){//当出现正中间往下时，挪回正规 
            i--;
            j = j + 2;
        }
        else if (i >= Image_N){//当出现往下突出时，挪回正规 
            i--;
            j = j + 2;
        }
        else if (j<0){//当出现往左突出时，挪回正规
            j++;
        }

        //向右上方向 
        for (; i >= 0 && j<Image_N; i--, j++){
            if (F_[i][j] == 0){
                count++;
            }
            else{
                char countString[Image_N*Image_N];
                _itoa_s(count, countString, 10);
                string strTemp = "/";
                strTemp = countString + strTemp;
                //cout<<"--"<<strTemp<<"--"<<endl;
                char SSSS_String[Image_N*Image_N];
                _itoa_s(SSSS[i][j], SSSS_String, 10);
                strTemp = strTemp + SSSS_String;
                //cout<<"**"<<strTemp<<"**"<<endl;

                //中间符号 
                ac_EntropyCoding_MiddleSymbol[index].R_S = strTemp;
                ac_EntropyCoding_MiddleSymbol[index].temp = F_[i][j];
                index++;
                count = 0;//置为0 ，计算下个不为0的数前面0的个数
            }
        }
        if (i<0 && j >= Image_N){//当出现正中间往上时，挪回正规 
            j--;
            i = i + 2;
        }
        else if (i<0){//当出现往上突出时，挪回正规 
            i++;
        }
        else if (j >= Image_N){//当出现往右突出时，挪回正规 
            j--;
            i = i + 2;
        }
    }

    //剩下半个周期的编码 
    //向左下方向 
    for (; i<Image_N&&j >= 0; i++, j--){
        if (F_[i][j] == 0){
            count++;
        }
        else{
            char countString[Image_N*Image_N];
            _itoa_s(count, countString, 10);
            string strTemp = "/";
            strTemp = countString + strTemp;
            //cout<<"--"<<strTemp<<"--"<<endl;
            char SSSS_String[Image_N*Image_N];
            _itoa_s(SSSS[i][j], SSSS_String, 10);
            strTemp = strTemp + SSSS_String;
            //cout<<"**"<<strTemp<<"**"<<endl;

            //中间符号 
            ac_EntropyCoding_MiddleSymbol[index].R_S = strTemp;
            ac_EntropyCoding_MiddleSymbol[index].temp = F_[i][j];
            index++;
            count = 0;//置为0 ，计算下个不为0的数前面0的个数
        }
    }
    if (i >= Image_N){//当出现往下突出时，挪回正规
        i--;
        j = j + 2;
    }
    if (F_[i][j] == 0){//最后一个点
        count++;
        ac_EntropyCoding_MiddleSymbol[index].R_S = "0/0(EOB)";
        ac_EntropyCoding_MiddleSymbol[index].temp = INT_MAX;
        index++;
    }
    else{
        char countString[Image_N*Image_N];
        _itoa_s(count, countString, 10);
        string strTemp = "/";
        strTemp = countString + strTemp;
        //cout<<"--"<<strTemp<<"--"<<endl;
        char SSSS_String[Image_N*Image_N];
        _itoa_s(SSSS[i][j], SSSS_String, 10);
        strTemp = strTemp + SSSS_String;
        //cout<<"**"<<strTemp<<"**"<<endl;

        //中间符号 
        ac_EntropyCoding_MiddleSymbol[index].R_S = strTemp + "(EOB)";
        ac_EntropyCoding_MiddleSymbol[index].temp = F_[i][j];
        index++;
    }

    /*测试*/
    //	for(int k=0;k<index;k++){
    //		cout<<ac_EntropyCoding_MiddleSymbol[k].R_S<<"\t"<<ac_EntropyCoding_MiddleSymbol[k].temp<<endl;
    //	}


    //对中间符号进行符号编码 
    //对R/S通过查亮度AC码表进行熵编码
    for (int u = 0; u<index; u++){
        for (int v = 0; v<stringMapList.partNum; v++){
            if (ac_EntropyCoding_MiddleSymbol[u].R_S == stringMapList.stringMap[v].key){
                ac_EntropyCodingStr[u].strTemp1 = stringMapList.stringMap[v].value;
            }
        }
        //对 temp进行转换成补码 
        //先将 temp转换成二进制串
        if (ac_EntropyCoding_MiddleSymbol[u].R_S != "0/0(EOB)"){
            ac_EntropyCodingStr[u].strTemp = TenToTwo(abs(ac_EntropyCoding_MiddleSymbol[u].temp));
            //转换成补码
            if (ac_EntropyCoding_MiddleSymbol[u].temp<0){
                ac_EntropyCodingStr[u].strTemp = ConvertToComplement(ac_EntropyCodingStr[u].strTemp);
            }

            /*测试*/
            //			cout<<"**********"<<ac_EntropyCodingStr[u].strTemp<<endl;
        }
        else{
            ac_EntropyCodingStr[u].strTemp = "" + '\0';

            /*测试*/
            //			cout<<"**********+"<<ac_EntropyCodingStr[u].strTemp<<endl;
        }
    }

    /*测试*/
    //	for(int k=0;k<index;k++){
    //		cout<<ac_EntropyCodingStr[k].strTemp1<<" "<<ac_EntropyCodingStr[k].strTemp<<endl;
    //	}
    return true;
}