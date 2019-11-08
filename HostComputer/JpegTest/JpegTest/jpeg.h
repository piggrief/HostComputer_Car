# pragma once 
# include <string>
#define MAX 100
#define Image_N 8 	//Image_NΪÿ��ͼ������ľ����С

using jpeg_string_t = std::string;

/*��������ֵ��*/
class  BrightnessQuantizedValueTable{
public:
    int Q[Image_N][Image_N];
    BrightnessQuantizedValueTable(){
        int x[Image_N][Image_N] = { 16, 11, 10, 16, 24, 40, 51, 61,
            12, 12, 14, 19, 26, 58, 60, 55,
            14, 13, 16, 24, 40, 57, 69, 56,
            14, 17, 22, 29, 51, 87, 80, 62,
            18, 22, 37, 56, 68, 109, 103, 77,
            24, 35, 55, 64, 81, 104, 113, 92,
            49, 64, 78, 87, 103, 121, 120, 101,
            72, 92, 95, 98, 112, 100, 103, 99 };
        for (int i = 0; i<Image_N; i++){
            for (int j = 0; j<Image_N; j++){
                Q[i][j] = x[i][j];
            }
        }
    }
};

/*����DC��ֵ���*/
class BrightnessDC_DifferenceTableList{
public:
    jpeg_string_t brightnessDC_DifferenceTable[12];//����DC��ֵ���  ���(�����±�)������ӳ��
    BrightnessDC_DifferenceTableList(){
        brightnessDC_DifferenceTable[0] = "00";
        brightnessDC_DifferenceTable[1] = "010";
        brightnessDC_DifferenceTable[2] = "011";
        brightnessDC_DifferenceTable[3] = "100";
        brightnessDC_DifferenceTable[4] = "101";
        brightnessDC_DifferenceTable[5] = "110";
        brightnessDC_DifferenceTable[6] = "1110";
        brightnessDC_DifferenceTable[7] = "11110";
        brightnessDC_DifferenceTable[8] = "111110";
        brightnessDC_DifferenceTable[9] = "1111110";
        brightnessDC_DifferenceTable[10] = "11111110";
        brightnessDC_DifferenceTable[11] = "111111110";
    }
};

/*ACϵ���ر���ʱ���м����*/
struct AC_EntropyCoding_MiddleSymbol{
    jpeg_string_t R_S;
    int temp;
};

/*�ر���ʱ�ı������*/
struct EntropyCoding{
    jpeg_string_t strTemp1;
    jpeg_string_t strTemp;
};

/* R/S������ӳ����*/
struct StringMap{
    jpeg_string_t key;
    jpeg_string_t value;
};

/*����AC���  R/S������ӳ���*/
/**�ر�ע�⣬���µ�����AC���ֻ�����ڸ����Ĳ������ݣ���Ҫ�����Ĳ������ݣ�����벹ȫ����AC���**/
//�����ҵ�һ��fantasy �Ĳ���http://menmory.blog.163.com/blog/static/12690012620114535032530/ �������бȽ���ϸ������AC���� 
class StringMapList{
public:
    StringMap stringMap[Image_N*Image_N];
    int partNum;	//������AC����е����� 
    StringMapList(){
        //���ֳ�������AC���
        stringMap[0].key = "0/0(EOB)";
        stringMap[0].value = "1010";
        stringMap[1].key = "0/1";
        stringMap[1].value = "00";
        stringMap[2].key = "1/1";
        stringMap[2].value = "1100";
        stringMap[3].key = "1/2";
        stringMap[3].value = "11011";
        stringMap[4].key = "2/1";
        stringMap[4].value = "11100";
        stringMap[5].key = "3/2";
        stringMap[5].value = "111110111";
        stringMap[6].key = "F/0(ZRL)";
        stringMap[6].value = "11111111001";
        stringMap[7].key = "F/F";
        stringMap[7].value = "1111111111111110";
        partNum = 8;
    }
};


extern BrightnessQuantizedValueTable brightnessQuantizedValueTable;//����һ����������ֵ��
extern BrightnessDC_DifferenceTableList brightnessDC_DifferenceTableList;//����һ������DC��ֵ���
extern AC_EntropyCoding_MiddleSymbol ac_EntropyCoding_MiddleSymbol[Image_N*Image_N];//�����ú������ؽṹ�����飬������ַ��������һЩ�޷���������룬�ʶ���Ϊȫ�ֱ��� 
extern EntropyCoding ac_EntropyCodingStr[Image_N*Image_N];//�����ú������ؽṹ�����飬������ַ��������һЩ�޷���������룬�ʶ���Ϊȫ�ֱ��� 
extern StringMapList stringMapList;//����һ�� ���ֳ�������AC���

/*DCϵ������*/
EntropyCoding DC_EntropyCoding(int &temp, int &temp1);
/*ACϵ������*/
bool AC_EntropyCoding(int F_[Image_N][Image_N], int &index);
/*��һ�������Ķ����ƴ���λȡ��*/
jpeg_string_t ConvertToComplement(jpeg_string_t strTemp);
/*��������������ת����ʮ����*/
int TwoToTen(jpeg_string_t strTemp);