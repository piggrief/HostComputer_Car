# include <iostream>
# include <cmath>
# include <stdio.h>
# include "jpeg.h"
using namespace std;
# define Image_Size 8

double image[Image_Size][Image_Size] =
{ { 46, 47, 52, 56, 57, 57, 52, 48 },
{ 46, 44, 47, 48, 45, 50, 47, 50 },
{ 50, 48, 48, 49, 49, 46, 49, 51 },
{ 50, 51, 54, 55, 55, 52, 49, 46 },
{ 50, 53, 59, 57, 51, 50, 53, 52 },
{ 48, 51, 56, 53, 46, 49, 53, 59 },
{ 47, 50, 52, 52, 53, 55, 50, 55 },
{ 49, 51, 51, 53, 58, 55, 55, 54 } };

int main()
{
    cout << "8x8分量Jpeg压缩" << endl;

    # pragma region 压缩编码
    const double PI = acos(-1);

    double ff[Image_Size][Image_Size], F[Image_Size][Image_Size];

    cout << "源图像的一个分量样本：" << endl;
    /*输出—源图像的一个分量样本*/
    for (int i = 0; i<Image_Size; i++){
        for (int j = 0; j<Image_Size; j++){
            cout << image[i][j] << "\t";
        }
        cout << endl;
    }

    //图像的一个分量样本-128后 
    for (int i = 0; i<Image_Size; i++){
        for (int j = 0; j<Image_Size; j++){
            ff[i][j] = image[i][j] - 128;
        }
    }

    cout << "源图像的一个分量样本-128后：" << endl;
    /*输出—图像的一个分量样本-128后*/
    for (int i = 0; i<Image_Size; i++){
        for (int j = 0; j<Image_Size; j++){
            cout << ff[i][j] << "\t";
        }
        cout << endl;
    }

    //由公式计算DCT变化后的系数矩阵
    for (int u = 0; u<Image_Size; u++){
        for (int v = 0; v<Image_Size; v++){
            double temp = 0.0;
            for (int i = 0; i<Image_Size; i++){
                for (int j = 0; j<Image_Size; j++){
                    temp = temp + ff[i][j] * cos((2 * i + 1)*u*PI*1.0 / 16)*cos((2 * j + 1)*v*PI*1.0 / 16);
                }
            }
            F[u][v] = 1.0 / 4 * (u == 0 ? 1.0 / sqrt(2) : 1)*(v == 0 ? 1.0 / sqrt(2) : 1)*temp;
        }
    }

    /*输出—DCT变化后的系数矩阵*/
    //DCT变化后的系数矩阵
    cout << "DCT变化后的系数矩阵:" << endl;
    for (int u = 0; u<Image_Size; u++){
        for (int v = 0; v<Image_Size; v++){
            printf("%.1f\t", F[u][v]);
            //cout<<F[u][v]<<" ";
        }
        cout << endl;
    }

    //利用公式将DCT变化后的系数矩阵转换为规格化量化系数矩阵 
    int F_[N][N];//规格化量化系数矩阵 
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){//二维数组Q 为亮度量化值表 
            F_[i][j] = (int)((F[i][j] / brightnessQuantizedValueTable.Q[i][j])>0.0) ? floor((F[i][j] / brightnessQuantizedValueTable.Q[i][j]) + 0.5) : ceil((F[i][j] / brightnessQuantizedValueTable.Q[i][j]) - 0.5);//进行量化，然后进行四舍五入 
        }
    }

    /*输出—规格化量化系数矩阵*/
    //规格化量化系数矩阵 
    cout << "规格化量化系数:" << endl;
    for (int u = 0; u<N; u++){
        for (int v = 0; v<N; v++){
            cout << F_[u][v] << "\t";
        }
        cout << endl;
    }

    //对DC系数生成中间符号(temp1,temp)
    int temp = F_[0][0];
    int temp1;
    EntropyCoding dc_EntropyCodingStr = DC_EntropyCoding(temp, temp1);

    cout << "中间符号：" << endl;
    /*输出—DC系数中间符号*/
    cout << temp1 << "\t" << temp << endl;

    int index = 0; //AC系数生成中间符号的个数 
    //对AC系数生成中间符号
    AC_EntropyCoding(F_, index);

    /*输出—AC系数中间符号*/
    for (int k = 0; k<index; k++){
        cout << ac_EntropyCoding_MiddleSymbol[k].R_S << "\t";
        if (ac_EntropyCoding_MiddleSymbol[k].R_S != "0/0(EOB)"){
            cout << ac_EntropyCoding_MiddleSymbol[k].temp;
        }
        cout << endl;
    }

    cout << "熵编码输出：" << endl;
    /*输出—DC系数熵编码输出*/
    cout << dc_EntropyCodingStr.strTemp1 << "\t" << dc_EntropyCodingStr.strTemp << endl;

    /*输出—AC系数熵编码输出*/
    for (int k = 0; k<index; k++){
        cout << ac_EntropyCodingStr[k].strTemp1 << "\t" << ac_EntropyCodingStr[k].strTemp << endl;
    }
    # pragma endregion

    # pragma region 解码
    cout << "----------------------------------------------------------------------" << endl << endl;
    cout << "解码过程：" << endl;
    //下面的解码过程只用到了压缩过程传输过来的熵编码和熵编码中AC系数的个数index(也可以通过一个循环来计算出这个index) 

    cout << "待解码的熵编码：" << endl;
    /*输出—DC系数熵编码输出*/
    cout << dc_EntropyCodingStr.strTemp1 << "\t" << dc_EntropyCodingStr.strTemp << endl;

    /*输出—AC系数熵编码输出*/
    for (int k = 0; k<index; k++){
        cout << ac_EntropyCodingStr[k].strTemp1 << "\t" << ac_EntropyCodingStr[k].strTemp << endl;
    }

    /*解码*/
    //将DC系数熵编码的编码转换为中间符号
    //对strTemp1进行反向查找亮度DC差值码表得到temp1
    int Itemp1;
    for (int i = 0; i<11; i++){
        if (dc_EntropyCodingStr.strTemp1 == brightnessDC_DifferenceTableList.brightnessDC_DifferenceTable[i]){
            Itemp1 = i;
        }
    }
    //对strTemp进行反向补码得到temp，可通过查找规律发现若是负数转换为补码后首个数字必为0，正数必不为0 
    int Itemp;
    if (dc_EntropyCodingStr.strTemp[0] == '0'){//为负数，先取反 
        string tempStr = ConvertToComplement(dc_EntropyCodingStr.strTemp);
        //转换为10进制
        Itemp = TwoToTen(tempStr);
        //加负号
        Itemp = -Itemp;
    }
    else{//为正数，直接转换为10进制即可 
        Itemp = TwoToTen(dc_EntropyCodingStr.strTemp);
    }

    cout << "中间符号：" << endl;
    /*输出—DC中间符号*/
    cout << Itemp1 << "\t" << Itemp << endl;

    //将AC系数熵编码的编码转换为中间符号
    AC_EntropyCoding_MiddleSymbol Iac_EntropyCoding_MiddleSymbol[N*N];
    //遍历所有的AC系数熵编码的编码对strTemp1进行反向查找亮度AC码表得到R_S
    for (int i = 0; i<index; i++){
        for (int u = 0; u<stringMapList.partNum; u++){
            if (ac_EntropyCodingStr[i].strTemp1 == stringMapList.stringMap[u].value){
                Iac_EntropyCoding_MiddleSymbol[i].R_S = stringMapList.stringMap[u].key;
            }
        }
        //对strTemp进行反向补码得到temp，可通过查找规律发现若是负数转换为补码后首个数字必为0，正数必不为0
        if (ac_EntropyCodingStr[i].strTemp[0] == '0'){//为负数，先取反 
            string tempStr = ConvertToComplement(ac_EntropyCodingStr[i].strTemp);
            //转换为10进制
            Iac_EntropyCoding_MiddleSymbol[i].temp = TwoToTen(tempStr);
            //加负号
            Iac_EntropyCoding_MiddleSymbol[i].temp = -Iac_EntropyCoding_MiddleSymbol[i].temp;
        }
        else{//为正数，直接转换为10进制即可 
            Iac_EntropyCoding_MiddleSymbol[i].temp = TwoToTen(ac_EntropyCodingStr[i].strTemp);
        }
    }

    /*输出—AC中间符号*/
    for (int i = 0; i<index; i++){
        cout << Iac_EntropyCoding_MiddleSymbol[i].R_S << "\t";
        if (Iac_EntropyCoding_MiddleSymbol[i].R_S != "0/0(EOB)"){
            cout << Iac_EntropyCoding_MiddleSymbol[i].temp;
        }
        cout << endl;
    }

    //规格化量化系数 
    int IF_[N][N];

    //初始化矩阵 
    for (int u = 0; u<N; u++){
        for (int v = 0; v<N; v++){
            IF_[u][v] = 0;
        }
    }

    //DC系数
    //还原编码 
    IF_[0][0] = Itemp;
    //AC系数
    //Z字形还原编码
    int count;//计算0的个数
    int a = 0, b = 1; //初始位置
    for (int h = 0; h<index; h++){
        //将/前的字符串转换为整数
        count = 0;
        for (int w = 0; Iac_EntropyCoding_MiddleSymbol[h].R_S[w] != '/'; w++){
            count = count * 10 + Iac_EntropyCoding_MiddleSymbol[h].R_S[w] - '0';
        }
        /*测试*/
        //		cout<<"count:"<<count<<endl; 

        while (Iac_EntropyCoding_MiddleSymbol[h].R_S != "0/0(EOB)"){
            //根据查找规律发现，当行+列为奇数时向左下方向，当行+列为偶数时为右上方向 
            if ((a + b) % 2 == 0){//偶数，向右上方向 
                for (; count >= 0 && a >= 0 && b<N; a--, b++){
                    if (count == 0){//此时放temp 
                        IF_[a][b] = Iac_EntropyCoding_MiddleSymbol[h].temp;
                        count--;
                        break;
                    }
                    else{//此时放0 
                        IF_[a][b] = 0;
                        count--;
                    }
                }
                if (count<0){//向右上移动 
                    a--;
                    b++;
                }
                if (a<0 && b >= N){//当出现正中间往上时，挪回正规  
                    b--;
                    a = a + 2;
                }
                else if (a<0){//当出现往上突出时，挪回正规
                    a++;
                }
                else if (b >= N){//当出现往右突出时，挪回正规 
                    b--;
                    a = a + 2;
                }
                if (count<0){//跳出到第一层循环 
                    break;
                }
            }
            else{//奇数，向左下方向 
                for (; count >= 0 && a<N&&b >= 0; a++, b--){
                    if (count == 0){//此时放temp 
                        IF_[a][b] = Iac_EntropyCoding_MiddleSymbol[h].temp;
                        count--;
                        break;
                    }
                    else{//此时放0 
                        IF_[a][b] = 0;
                        count--;
                    }
                }
                if (count<0){//向左下移动 
                    a++;
                    b--;
                }
                if (a >= N&&b<0){//当出现正中间往下时，挪回正规 
                    a--;
                    b = b + 2;
                }
                else if (a >= N){//当出现往下突出时，挪回正规 
                    a--;
                    b = b + 2;
                }
                else if (b<0){//当出现往左突出时，挪回正规
                    b++;
                }
                if (count<0){//跳出到第一层循环 
                    break;
                }
            }
        }
    }

    /*输出—规格化量化系数矩阵*/
    //规格化量化系数矩阵 
    cout << "规格化量化系数:" << endl;
    for (int u = 0; u<N; u++){
        for (int v = 0; v<N; v++){
            cout << IF_[u][v] << "\t";
        }
        cout << endl;
    }


    //利用公式将规格化量化系数矩阵转换为逆量化后的系数矩阵  
    double IF[N][N];//逆量化后的系数矩阵
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){//二维数组Q 为亮度量化值表
            IF[i][j] = 1.0*IF_[i][j] * brightnessQuantizedValueTable.Q[i][j];
        }
    }

    /*输出—逆量化后的系数矩阵*/
    cout << "逆量化后的系数矩阵:" << endl;
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){
            cout << IF[i][j] << "\t";
        }
        cout << endl;
    }

    //由公式计算IDCT变化后的系数矩阵
    double Iff[N][N];
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){
            double sum = 0.0;
            for (int u = 0; u<N; u++){
                for (int v = 0; v<N; v++){
                    sum = sum + (u == 0 ? 1.0 / sqrt(2.0) : 1.0)*(v == 0 ? 1.0 / sqrt(2.0) : 1.0)*IF[u][v] * cos((2 * i + 1)*u*PI*1.0 / 16)*cos((2 * j + 1)*v*PI*1.0 / 16);
                }
            }
            Iff[i][j] = 1.0 / 4 * sum;
        }
    }

    /*输出—IDCT变化后的系数矩阵*/
    cout << "IDCT变化后的系数矩阵:" << endl;
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){
            //cout<<Iff[i][j]<<"\t";
            printf("%.0f\t", Iff[i][j]);
        }
        cout << endl;
    }


    cout << "源图像的一个分量样本的重构图像：" << endl;
    /*IDCT变化后的系数矩阵+128后变成源图像的一个分量样本的重构图像*/
    double If[N][N];
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){
            If[i][j] = Iff[i][j] + 128;
        }
    }

    /*输出—源图像的一个分量样本的重构图像*/
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){
            //cout<<If[i][j]<<"\t";
            printf("%.0f\t", If[i][j]);
        }
        cout << endl;
    }
    # pragma endregion

    while (true)
    {

    }
    return 0;
}
