# pragma once 
# include <string>
#define MAX 100
#define Image_N 8 	//Image_N为每个图像分量的矩阵大小

using jpeg_string_t = std::string;

/*亮度量化值表*/
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

/*亮度DC差值码表*/
class BrightnessDC_DifferenceTableList{
public:
    jpeg_string_t brightnessDC_DifferenceTable[12];//亮度DC差值码表  类别(数组下标)与码字映射
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

/*AC系数熵编码时的中间符号*/
struct AC_EntropyCoding_MiddleSymbol{
    jpeg_string_t R_S;
    int temp;
};

/*熵编码时的编码输出*/
struct EntropyCoding{
    jpeg_string_t strTemp1;
    jpeg_string_t strTemp;
};

/* R/S与码字映射结点*/
struct StringMap{
    jpeg_string_t key;
    jpeg_string_t value;
};

/*亮度AC码表  R/S与码字映射表*/
/**特别注意，以下的亮度AC码表只适用于给出的测试数据，如要其它的测试数据，则必须补全亮度AC码表**/
//网上找到一个fantasy 的博客http://menmory.blog.163.com/blog/static/12690012620114535032530/ 这里面有比较详细的亮度AC码表等 
class StringMapList{
public:
    StringMap stringMap[Image_N*Image_N];
    int partNum;	//该亮度AC码表中的条数 
    StringMapList(){
        //部分常用亮度AC码表
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


extern BrightnessQuantizedValueTable brightnessQuantizedValueTable;//定义一个亮度量化值表
extern BrightnessDC_DifferenceTableList brightnessDC_DifferenceTableList;//定义一个亮度DC差值码表
extern AC_EntropyCoding_MiddleSymbol ac_EntropyCoding_MiddleSymbol[Image_N*Image_N];//由于用函数返回结构体数组，里面的字符串会出现一些无法处理的乱码，故定义为全局变量 
extern EntropyCoding ac_EntropyCodingStr[Image_N*Image_N];//由于用函数返回结构体数组，里面的字符串会出现一些无法处理的乱码，故定义为全局变量 
extern StringMapList stringMapList;//定义一个 部分常用亮度AC码表

/*DC系数编码*/
EntropyCoding DC_EntropyCoding(int &temp, int &temp1);
/*AC系数编码*/
bool AC_EntropyCoding(int F_[Image_N][Image_N], int &index);
/*将一个负数的二进制串逐位取反*/
jpeg_string_t ConvertToComplement(jpeg_string_t strTemp);
/*将正整数二进制转换成十进制*/
int TwoToTen(jpeg_string_t strTemp);