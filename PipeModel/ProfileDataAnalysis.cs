using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeModel.DataAnalysis
{
    class ProfileDataAnalysis
    {
        /// <summary>
        /// 单次扫描轮廓（x轴）数据处理
        /// </summary>
        /// <param name="profDatas"></param>
        /// <returns></returns>
        public static int[] DealWithSingProfileData(int[] profDatas)
        {
            int left = 0;
            int right = profDatas.Length - 1;
            for (int i = left; i <= right; i++)
                profDatas[i] = Convert.ToInt32(300 - profDatas[i] * 0.00001);

            ///数据两头去无效数据
            while (left <= right && profDatas[left] == Define.INVALID_DATA)
            {
                profDatas[left++] = Define.PROFILE_MIN_Y;
            }
            while (left <= right && profDatas[right] == Define.INVALID_DATA)
            {
                profDatas[right--] = Define.PROFILE_MIN_Y;
            }
            ///中间无效数据处理
            for (int i = left; i < 400; i++)
            {
                if (profDatas[i] == Define.INVALID_DATA)
                    profDatas[i] = profDatas[i - 1];
            }
            for (int i = right; i > 400; i--)
            {
                if (profDatas[i] == Define.INVALID_DATA)
                    profDatas[i] = profDatas[i + 1];
            }
           
            return profDatas;
        }



        /// <summary>
        /// 高速扫描轮廓（x轴）数据处理
        /// </summary>
        /// <param name="profDatas"></param>
        /// <returns></returns>
        public static List<int[]> DealWithHightProfileData(List<int[]> data)
        {
            if (data.Count <= 0)
                return null;
            List<int[]> dataRs = new List<int[]>(data.Count);
            for (int p = 0; p < data.Count; p++)
            {
                int[] profDatas = data[p];
                int left = 6;
                int right = profDatas.Length - 2;
                for (int i = left; i <= right; i++)
                    profDatas[i] = Convert.ToInt32(300 - profDatas[i] * 0.00001);

                //去无效数据
                int temp = left;
                while (temp <= right)
                {
                    if (profDatas[temp] == Define.INVALID_DATA)
                        profDatas[temp] = Define.PROFILE_MIN_Y;
                    temp++;
                }

                //当距离超过400mm，将值置为0
                for (int i = left; i < right; i++)
                {
                    if (profDatas[i] > 400)
                        profDatas[i] = 0;
                }
                

                /////数据两头去无效数据
                //while (left <= right && profDatas[left] == Define.INVALID_DATA)
                //{
                //    profDatas[left++] = Define.PROFILE_MIN_Y;
                //}
                //while (left <= right && profDatas[right] == Define.INVALID_DATA)
                //{
                //    profDatas[right--] = Define.PROFILE_MIN_Y;
                //}

                /////中间无效数据处理
                //for (int i = left; i < 406; i++)
                //{
                //    if (profDatas[i] == Define.INVALID_DATA)
                //        profDatas[i] = profDatas[i - 1];
                //}
                //for (int i = right; i >= 406; i--)
                //{
                //    if (profDatas[i] == Define.INVALID_DATA)
                //        profDatas[i] = profDatas[i + 1];
                //}


                dataRs.Add(profDatas);
            }
            return dataRs;
        }


    }
}
