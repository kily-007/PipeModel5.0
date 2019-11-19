﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeModel
{
    //错位扫描计算
    /// <summary>
    /// 管道总宽度为：
    /// </summary>
    public static class MisalignedScan
    {
        /// <summary>
        /// 数据分析
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        public static double[] MisalignedAnalizeData(List<int[]> datas,double k)
        {
            if (datas.Count <= 0)
                return null;
            List<int[]> MisaX = new List<int[]>();
            List<int[]> MisaY = new List<int[]>();
            List<int> MisaZ = new List<int>();
            int leftTx = 0;
            int rightTx = 0;
            int middley = 0;
            int leftSumy = 0;
            int middleSumy = 0;
            int rightSumy = 0;
            bool isF = false;
            for (int i = 0; i < datas.Count; i++)
            {
                leftTx = 6;
                rightTx = 806;
                middley = 0;
                leftSumy = 0;
                middleSumy = 0;
                rightSumy = 0;

                
                while (leftTx < rightTx && datas[i][leftTx] == Define.INVALID_DATA_ORIGINAL) leftTx++;
                while (leftTx < rightTx && datas[i][rightTx] == Define.INVALID_DATA_ORIGINAL) rightTx--;

                //根据参数，自动补正数据
                for (int j = leftTx; j <= rightTx; j++)
                {
                    datas[i][j] -= (int)(k * (j - 6));
                }
                //有效点的个数
                if ( scanWidth(leftTx,rightTx,datas[i]))
                {
                    int[] MisaXArra = new int[2] { 0, 0 };
                    MisaX.Add(MisaXArra);
                    int[] MisaYArra = new int[3] { 0, 0, 0 };
                    MisaY.Add(MisaYArra);
                    MisaZ.Add(1);
                    isF = true;
                }
                else
                {   //MisaX方向磨损计算
                    int[] MisaXArray = new int[2] { leftTx-6, rightTx-6 };
                    MisaX.Add(MisaXArray);
                    //MisaY方向磨损计算0
                    middley = (int)(leftTx + rightTx) / 2;
                    for (int j = 0; j < 10; j++)
                    {
                        leftSumy += datas[i][leftTx + j];
                        middleSumy += datas[i][middley + j];
                        rightSumy += datas[i][rightTx - j];
                    }
                    int[] MisaYArray = new int[3] { leftSumy / 10, middleSumy / 10, rightSumy / 10 };
                    MisaY.Add(MisaYArray);
                    MisaZ.Add(0);

                }
                
            }
            MysqlConnection.executeInsert(MisaX, MisaY, MisaZ, "S");
            double[] rs;
            if (isF)
                rs = new double[4] {(MisaY[100][0]+ MisaY[100][2])/2 - MisaY[100][1], 0, 0, 0 };//misa，left，Middle，Right
            else
                rs = new double[4] { (MisaY[100][0] + MisaY[100][2]) / 2 - MisaY[100][1], MisaX[0][0], MisaY[100][1], MisaX[0][1] };//misa，left，Middle，Right
            return rs;
            //return MisaY[100][1];
        }

        //判断宽度小于300，则为缝隙
        public static bool scanWidth(int left,int right,int[] data)
        {
            if (right - left < 300)//宽度小于300
            {
                return true;
            }
            else
            {
                int cout = 0;
                for (int i = left; i < right; i++)
                {
                    if (data[i] != 0)
                        cout++;
                }
                if (cout < 300)
                    return true;
            }
            return false;
        }
    }
}
