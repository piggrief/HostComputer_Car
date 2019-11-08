# include <string>
# include "jpeg.h"
using namespace std;

BrightnessQuantizedValueTable brightnessQuantizedValueTable;//����һ����������ֵ��

BrightnessDC_DifferenceTableList brightnessDC_DifferenceTableList;//����һ������DC��ֵ���

AC_EntropyCoding_MiddleSymbol ac_EntropyCoding_MiddleSymbol[Image_N*Image_N];//�����ú������ؽṹ�����飬������ַ��������һЩ�޷���������룬�ʶ���Ϊȫ�ֱ��� 

EntropyCoding ac_EntropyCodingStr[Image_N*Image_N];//�����ú������ؽṹ�����飬������ַ��������һЩ�޷���������룬�ʶ���Ϊȫ�ֱ��� 

StringMapList stringMapList;//����һ�� ���ֳ�������AC���


/*DC��ֵ��Χ������ͨ���Ա��ҹ��ɷ������¹��� */
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


/*ACϵ����Χ��,����ͨ���Ա��ҹ��ɷ������¹���*/
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


/*��������ʮ����ת���ɶ�����*/
string TenToTwo(int temp){
    string strTemp = "";
    //�ɷ��� 
    //	for(int k=abs(temp);k>0;k=k/2){
    //		strTemp = strTemp + (k%2==1?'1':'0');
    //	}
    //	//���� 
    //	int len = strTemp.length();
    //	for(int k=0;k<len/2;k++){
    //		char t = strTemp[k];
    //		strTemp[k] = strTemp[len-1-k];
    //		strTemp[len-1-k] = t;
    //	}
    //�·��� 
    char str[Image_N*Image_N];
    _itoa_s(temp, str, 2);
    strTemp = str;
    return strTemp;
}


/*��������������ת����ʮ����*/
int TwoToTen(string strTemp){
    int temp = 0;
    for (int i = 0; i<strTemp.length(); i++){
        temp = temp * 2 + strTemp[i] - '0';
    }
    return temp;
}

/*��һ�������Ķ����ƴ���λȡ��*/
string ConvertToComplement(string strTemp){
    string str = "";
    for (int i = 0; i<strTemp.length(); i++){
        str = str + (strTemp[i] == '1' ? '0' : '1');
    }
    return str;
}

/*DCϵ������*/
EntropyCoding DC_EntropyCoding(int &temp, int &temp1){
    //��DCϵ�������м���ţ��м����(temp1,temp)��

    //��DC��ֵ��
    temp1 = DC_Difference(temp);

    /*����*/
    //	cout<<temp1<<endl;

    //���м����ͨ�������з��ű��� 
    //�� temp1ͨ��������DC��ֵ�������ر���
    EntropyCoding dc_EntropyCodingStr;
    dc_EntropyCodingStr.strTemp1 = brightnessDC_DifferenceTableList.brightnessDC_DifferenceTable[temp1];

    //�� temp����ת���ɲ��� 
    //�Ƚ� tempת���ɶ����ƴ�
    dc_EntropyCodingStr.strTemp = TenToTwo(abs(temp));
    //ת���ɲ���
    if (temp<0){
        dc_EntropyCodingStr.strTemp = ConvertToComplement(dc_EntropyCodingStr.strTemp);
    }

    /*����*/
    //	cout<<dc_EntropyCodingStr.strTemp1<<"\t"<<dc_EntropyCodingStr.strTemp<<endl;

    return dc_EntropyCodingStr;
}


/*ACϵ������*/
bool AC_EntropyCoding(int F_[Image_N][Image_N], int &index){
    //��ACϵ�������м������/��Ĳ��� 
    int SSSS[Image_N][Image_N];

    //��ACϵ����Χ��,����ͨ���Ա��ҹ��ɷ������¹��� 
    for (int i = 0; i<Image_N; i++){
        for (int j = 0; j<Image_N; j++){
            SSSS[i][j] = AC_Difference(F_[i][j]);
        }
    }

    /*����*/
    //	for(int i=0;i<Image_N;i++){
    //		for(int j=0;j<Image_N;j++){
    //			cout<<SSSS[i][j]<<" ";
    //		}
    //		cout<<endl;
    //	}

    //Z���α���
    int count = 0;//����0�ĸ���
    int i, j, t;
    for (i = 0, j = 1, t = 1; t <= Image_N - 2; t++){//���������Ϊһ�����ڣ����Ҫִ��Image_N-2�����ڣ�����Image_N=8��ͨ���۲췢��ÿһ��һ��Ϊһ���ڣ�����6������+��Σ� 
        //�����·��� 
        for (; i<Image_N&&j >= 0; i++, j--){
            if (F_[i][j] == 0){
                count++;
            }
            else{
                char countString[Image_N*Image_N];
                _itoa_s(count, countString, 10);//������countת��Ϊ�ַ�����������countString����10���Ʒ�ʽ��Ҳ��ָ��2��8��10��16�Ƚ���ʵ�ֽ���ת��������ת�����淨�� 
                string strTemp = "/";
                strTemp = countString + strTemp;
                //cout<<"--"<<strTemp<<"--"<<endl;
                char SSSS_String[Image_N*Image_N];
                _itoa_s(SSSS[i][j], SSSS_String, 10);
                strTemp = strTemp + SSSS_String;
                //cout<<"**"<<strTemp<<"**"<<endl;

                //�м���� 
                ac_EntropyCoding_MiddleSymbol[index].R_S = strTemp;
                ac_EntropyCoding_MiddleSymbol[index].temp = F_[i][j];
                index++;
                count = 0;//��Ϊ0 �������¸���Ϊ0����ǰ��0�ĸ��� 
            }
        }
        if (i >= Image_N&&j<0){//���������м�����ʱ��Ų������ 
            i--;
            j = j + 2;
        }
        else if (i >= Image_N){//����������ͻ��ʱ��Ų������ 
            i--;
            j = j + 2;
        }
        else if (j<0){//����������ͻ��ʱ��Ų������
            j++;
        }

        //�����Ϸ��� 
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

                //�м���� 
                ac_EntropyCoding_MiddleSymbol[index].R_S = strTemp;
                ac_EntropyCoding_MiddleSymbol[index].temp = F_[i][j];
                index++;
                count = 0;//��Ϊ0 �������¸���Ϊ0����ǰ��0�ĸ���
            }
        }
        if (i<0 && j >= Image_N){//���������м�����ʱ��Ų������ 
            j--;
            i = i + 2;
        }
        else if (i<0){//����������ͻ��ʱ��Ų������ 
            i++;
        }
        else if (j >= Image_N){//����������ͻ��ʱ��Ų������ 
            j--;
            i = i + 2;
        }
    }

    //ʣ�°�����ڵı��� 
    //�����·��� 
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

            //�м���� 
            ac_EntropyCoding_MiddleSymbol[index].R_S = strTemp;
            ac_EntropyCoding_MiddleSymbol[index].temp = F_[i][j];
            index++;
            count = 0;//��Ϊ0 �������¸���Ϊ0����ǰ��0�ĸ���
        }
    }
    if (i >= Image_N){//����������ͻ��ʱ��Ų������
        i--;
        j = j + 2;
    }
    if (F_[i][j] == 0){//���һ����
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

        //�м���� 
        ac_EntropyCoding_MiddleSymbol[index].R_S = strTemp + "(EOB)";
        ac_EntropyCoding_MiddleSymbol[index].temp = F_[i][j];
        index++;
    }

    /*����*/
    //	for(int k=0;k<index;k++){
    //		cout<<ac_EntropyCoding_MiddleSymbol[k].R_S<<"\t"<<ac_EntropyCoding_MiddleSymbol[k].temp<<endl;
    //	}


    //���м���Ž��з��ű��� 
    //��R/Sͨ��������AC�������ر���
    for (int u = 0; u<index; u++){
        for (int v = 0; v<stringMapList.partNum; v++){
            if (ac_EntropyCoding_MiddleSymbol[u].R_S == stringMapList.stringMap[v].key){
                ac_EntropyCodingStr[u].strTemp1 = stringMapList.stringMap[v].value;
            }
        }
        //�� temp����ת���ɲ��� 
        //�Ƚ� tempת���ɶ����ƴ�
        if (ac_EntropyCoding_MiddleSymbol[u].R_S != "0/0(EOB)"){
            ac_EntropyCodingStr[u].strTemp = TenToTwo(abs(ac_EntropyCoding_MiddleSymbol[u].temp));
            //ת���ɲ���
            if (ac_EntropyCoding_MiddleSymbol[u].temp<0){
                ac_EntropyCodingStr[u].strTemp = ConvertToComplement(ac_EntropyCodingStr[u].strTemp);
            }

            /*����*/
            //			cout<<"**********"<<ac_EntropyCodingStr[u].strTemp<<endl;
        }
        else{
            ac_EntropyCodingStr[u].strTemp = "" + '\0';

            /*����*/
            //			cout<<"**********+"<<ac_EntropyCodingStr[u].strTemp<<endl;
        }
    }

    /*����*/
    //	for(int k=0;k<index;k++){
    //		cout<<ac_EntropyCodingStr[k].strTemp1<<" "<<ac_EntropyCodingStr[k].strTemp<<endl;
    //	}
    return true;
}