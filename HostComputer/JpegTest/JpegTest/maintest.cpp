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
    cout << "8x8����Jpegѹ��" << endl;

    # pragma region ѹ������
    const double PI = acos(-1);

    double ff[Image_Size][Image_Size], F[Image_Size][Image_Size];

    cout << "Դͼ���һ������������" << endl;
    /*�����Դͼ���һ����������*/
    for (int i = 0; i<Image_Size; i++){
        for (int j = 0; j<Image_Size; j++){
            cout << image[i][j] << "\t";
        }
        cout << endl;
    }

    //ͼ���һ����������-128�� 
    for (int i = 0; i<Image_Size; i++){
        for (int j = 0; j<Image_Size; j++){
            ff[i][j] = image[i][j] - 128;
        }
    }

    cout << "Դͼ���һ����������-128��" << endl;
    /*�����ͼ���һ����������-128��*/
    for (int i = 0; i<Image_Size; i++){
        for (int j = 0; j<Image_Size; j++){
            cout << ff[i][j] << "\t";
        }
        cout << endl;
    }

    //�ɹ�ʽ����DCT�仯���ϵ������
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

    /*�����DCT�仯���ϵ������*/
    //DCT�仯���ϵ������
    cout << "DCT�仯���ϵ������:" << endl;
    for (int u = 0; u<Image_Size; u++){
        for (int v = 0; v<Image_Size; v++){
            printf("%.1f\t", F[u][v]);
            //cout<<F[u][v]<<" ";
        }
        cout << endl;
    }

    //���ù�ʽ��DCT�仯���ϵ������ת��Ϊ�������ϵ������ 
    int F_[N][N];//�������ϵ������ 
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){//��ά����Q Ϊ��������ֵ�� 
            F_[i][j] = (int)((F[i][j] / brightnessQuantizedValueTable.Q[i][j])>0.0) ? floor((F[i][j] / brightnessQuantizedValueTable.Q[i][j]) + 0.5) : ceil((F[i][j] / brightnessQuantizedValueTable.Q[i][j]) - 0.5);//����������Ȼ������������� 
        }
    }

    /*������������ϵ������*/
    //�������ϵ������ 
    cout << "�������ϵ��:" << endl;
    for (int u = 0; u<N; u++){
        for (int v = 0; v<N; v++){
            cout << F_[u][v] << "\t";
        }
        cout << endl;
    }

    //��DCϵ�������м����(temp1,temp)
    int temp = F_[0][0];
    int temp1;
    EntropyCoding dc_EntropyCodingStr = DC_EntropyCoding(temp, temp1);

    cout << "�м���ţ�" << endl;
    /*�����DCϵ���м����*/
    cout << temp1 << "\t" << temp << endl;

    int index = 0; //ACϵ�������м���ŵĸ��� 
    //��ACϵ�������м����
    AC_EntropyCoding(F_, index);

    /*�����ACϵ���м����*/
    for (int k = 0; k<index; k++){
        cout << ac_EntropyCoding_MiddleSymbol[k].R_S << "\t";
        if (ac_EntropyCoding_MiddleSymbol[k].R_S != "0/0(EOB)"){
            cout << ac_EntropyCoding_MiddleSymbol[k].temp;
        }
        cout << endl;
    }

    cout << "�ر��������" << endl;
    /*�����DCϵ���ر������*/
    cout << dc_EntropyCodingStr.strTemp1 << "\t" << dc_EntropyCodingStr.strTemp << endl;

    /*�����ACϵ���ر������*/
    for (int k = 0; k<index; k++){
        cout << ac_EntropyCodingStr[k].strTemp1 << "\t" << ac_EntropyCodingStr[k].strTemp << endl;
    }
    # pragma endregion

    # pragma region ����
    cout << "----------------------------------------------------------------------" << endl << endl;
    cout << "������̣�" << endl;
    //����Ľ������ֻ�õ���ѹ�����̴���������ر�����ر�����ACϵ���ĸ���index(Ҳ����ͨ��һ��ѭ������������index) 

    cout << "��������ر��룺" << endl;
    /*�����DCϵ���ر������*/
    cout << dc_EntropyCodingStr.strTemp1 << "\t" << dc_EntropyCodingStr.strTemp << endl;

    /*�����ACϵ���ر������*/
    for (int k = 0; k<index; k++){
        cout << ac_EntropyCodingStr[k].strTemp1 << "\t" << ac_EntropyCodingStr[k].strTemp << endl;
    }

    /*����*/
    //��DCϵ���ر���ı���ת��Ϊ�м����
    //��strTemp1���з����������DC��ֵ���õ�temp1
    int Itemp1;
    for (int i = 0; i<11; i++){
        if (dc_EntropyCodingStr.strTemp1 == brightnessDC_DifferenceTableList.brightnessDC_DifferenceTable[i]){
            Itemp1 = i;
        }
    }
    //��strTemp���з�����õ�temp����ͨ�����ҹ��ɷ������Ǹ���ת��Ϊ������׸����ֱ�Ϊ0�������ز�Ϊ0 
    int Itemp;
    if (dc_EntropyCodingStr.strTemp[0] == '0'){//Ϊ��������ȡ�� 
        string tempStr = ConvertToComplement(dc_EntropyCodingStr.strTemp);
        //ת��Ϊ10����
        Itemp = TwoToTen(tempStr);
        //�Ӹ���
        Itemp = -Itemp;
    }
    else{//Ϊ������ֱ��ת��Ϊ10���Ƽ��� 
        Itemp = TwoToTen(dc_EntropyCodingStr.strTemp);
    }

    cout << "�м���ţ�" << endl;
    /*�����DC�м����*/
    cout << Itemp1 << "\t" << Itemp << endl;

    //��ACϵ���ر���ı���ת��Ϊ�м����
    AC_EntropyCoding_MiddleSymbol Iac_EntropyCoding_MiddleSymbol[N*N];
    //�������е�ACϵ���ر���ı����strTemp1���з����������AC���õ�R_S
    for (int i = 0; i<index; i++){
        for (int u = 0; u<stringMapList.partNum; u++){
            if (ac_EntropyCodingStr[i].strTemp1 == stringMapList.stringMap[u].value){
                Iac_EntropyCoding_MiddleSymbol[i].R_S = stringMapList.stringMap[u].key;
            }
        }
        //��strTemp���з�����õ�temp����ͨ�����ҹ��ɷ������Ǹ���ת��Ϊ������׸����ֱ�Ϊ0�������ز�Ϊ0
        if (ac_EntropyCodingStr[i].strTemp[0] == '0'){//Ϊ��������ȡ�� 
            string tempStr = ConvertToComplement(ac_EntropyCodingStr[i].strTemp);
            //ת��Ϊ10����
            Iac_EntropyCoding_MiddleSymbol[i].temp = TwoToTen(tempStr);
            //�Ӹ���
            Iac_EntropyCoding_MiddleSymbol[i].temp = -Iac_EntropyCoding_MiddleSymbol[i].temp;
        }
        else{//Ϊ������ֱ��ת��Ϊ10���Ƽ��� 
            Iac_EntropyCoding_MiddleSymbol[i].temp = TwoToTen(ac_EntropyCodingStr[i].strTemp);
        }
    }

    /*�����AC�м����*/
    for (int i = 0; i<index; i++){
        cout << Iac_EntropyCoding_MiddleSymbol[i].R_S << "\t";
        if (Iac_EntropyCoding_MiddleSymbol[i].R_S != "0/0(EOB)"){
            cout << Iac_EntropyCoding_MiddleSymbol[i].temp;
        }
        cout << endl;
    }

    //�������ϵ�� 
    int IF_[N][N];

    //��ʼ������ 
    for (int u = 0; u<N; u++){
        for (int v = 0; v<N; v++){
            IF_[u][v] = 0;
        }
    }

    //DCϵ��
    //��ԭ���� 
    IF_[0][0] = Itemp;
    //ACϵ��
    //Z���λ�ԭ����
    int count;//����0�ĸ���
    int a = 0, b = 1; //��ʼλ��
    for (int h = 0; h<index; h++){
        //��/ǰ���ַ���ת��Ϊ����
        count = 0;
        for (int w = 0; Iac_EntropyCoding_MiddleSymbol[h].R_S[w] != '/'; w++){
            count = count * 10 + Iac_EntropyCoding_MiddleSymbol[h].R_S[w] - '0';
        }
        /*����*/
        //		cout<<"count:"<<count<<endl; 

        while (Iac_EntropyCoding_MiddleSymbol[h].R_S != "0/0(EOB)"){
            //���ݲ��ҹ��ɷ��֣�����+��Ϊ����ʱ�����·��򣬵���+��Ϊż��ʱΪ���Ϸ��� 
            if ((a + b) % 2 == 0){//ż���������Ϸ��� 
                for (; count >= 0 && a >= 0 && b<N; a--, b++){
                    if (count == 0){//��ʱ��temp 
                        IF_[a][b] = Iac_EntropyCoding_MiddleSymbol[h].temp;
                        count--;
                        break;
                    }
                    else{//��ʱ��0 
                        IF_[a][b] = 0;
                        count--;
                    }
                }
                if (count<0){//�������ƶ� 
                    a--;
                    b++;
                }
                if (a<0 && b >= N){//���������м�����ʱ��Ų������  
                    b--;
                    a = a + 2;
                }
                else if (a<0){//����������ͻ��ʱ��Ų������
                    a++;
                }
                else if (b >= N){//����������ͻ��ʱ��Ų������ 
                    b--;
                    a = a + 2;
                }
                if (count<0){//��������һ��ѭ�� 
                    break;
                }
            }
            else{//�����������·��� 
                for (; count >= 0 && a<N&&b >= 0; a++, b--){
                    if (count == 0){//��ʱ��temp 
                        IF_[a][b] = Iac_EntropyCoding_MiddleSymbol[h].temp;
                        count--;
                        break;
                    }
                    else{//��ʱ��0 
                        IF_[a][b] = 0;
                        count--;
                    }
                }
                if (count<0){//�������ƶ� 
                    a++;
                    b--;
                }
                if (a >= N&&b<0){//���������м�����ʱ��Ų������ 
                    a--;
                    b = b + 2;
                }
                else if (a >= N){//����������ͻ��ʱ��Ų������ 
                    a--;
                    b = b + 2;
                }
                else if (b<0){//����������ͻ��ʱ��Ų������
                    b++;
                }
                if (count<0){//��������һ��ѭ�� 
                    break;
                }
            }
        }
    }

    /*������������ϵ������*/
    //�������ϵ������ 
    cout << "�������ϵ��:" << endl;
    for (int u = 0; u<N; u++){
        for (int v = 0; v<N; v++){
            cout << IF_[u][v] << "\t";
        }
        cout << endl;
    }


    //���ù�ʽ���������ϵ������ת��Ϊ���������ϵ������  
    double IF[N][N];//���������ϵ������
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){//��ά����Q Ϊ��������ֵ��
            IF[i][j] = 1.0*IF_[i][j] * brightnessQuantizedValueTable.Q[i][j];
        }
    }

    /*��������������ϵ������*/
    cout << "���������ϵ������:" << endl;
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){
            cout << IF[i][j] << "\t";
        }
        cout << endl;
    }

    //�ɹ�ʽ����IDCT�仯���ϵ������
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

    /*�����IDCT�仯���ϵ������*/
    cout << "IDCT�仯���ϵ������:" << endl;
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){
            //cout<<Iff[i][j]<<"\t";
            printf("%.0f\t", Iff[i][j]);
        }
        cout << endl;
    }


    cout << "Դͼ���һ�������������ع�ͼ��" << endl;
    /*IDCT�仯���ϵ������+128����Դͼ���һ�������������ع�ͼ��*/
    double If[N][N];
    for (int i = 0; i<N; i++){
        for (int j = 0; j<N; j++){
            If[i][j] = Iff[i][j] + 128;
        }
    }

    /*�����Դͼ���һ�������������ع�ͼ��*/
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
