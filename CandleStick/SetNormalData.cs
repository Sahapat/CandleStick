﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CandleStick
{
    class SetNormalData
    {
        private CandleStatus[] status = new CandleStatus[0];
        private CandleNormalData[] normalData = new CandleNormalData[0];
        private int StartPoint;
        private int SizePerUnit = 1; //It's mean 1 point per unit

        public CandleNormalData[] getNormalData(BigInteger[] package,int startPoint)
        {
            InitComponent(package,startPoint);
            SetNormal();
            Shifting();
            return normalData;
        }
        private void InitComponent(BigInteger[] package,int startPoint)
        {
            Array.Resize<CandleStatus>(ref status, package.Length);
            Array.Resize<CandleNormalData>(ref normalData, package.Length);
            StartPoint = startPoint;
            for (int i = 0; i < package.Length; i++)
            {
                status[i] = Package.UnPack(package[i]);
            }
        }
        private void SetNormal()
        {
            for (int i = 0; i < normalData.Length; i++)
            {
                normalData[i].Low = StartPoint;
                if (status[i].Direction == (int)TypeTrend.UP)
                {
                    normalData[i].Open = normalData[i].Low + (status[i].LowerShadow * SizePerUnit);
                    normalData[i].Close = normalData[i].Open + (status[i].Body * SizePerUnit);
                    normalData[i].High = normalData[i].Close + (status[i].UpperShadow * SizePerUnit);
                }
                else
                {
                    normalData[i].Close = normalData[i].Low + (status[i].LowerShadow * SizePerUnit);
                    normalData[i].Open = normalData[i].Close + (status[i].Body * SizePerUnit);
                    normalData[i].High = normalData[i].Close + (status[i].UpperShadow * SizePerUnit);
                }
            }
        }
        private void Shifting()
        {
            for(int i = 1;i<=normalData.Length-1;i++)
            {
                SettingDefaultCandle(i);
                if (status[i].GAP == (int)CandleGAP.GAP)
                {
                    if (status[i].HigherHigh == 1)
                    {
                        while (normalData[i].Low <= normalData[i - 1].High)
                        {
                            ShiftUp(i);
                        }
                    }
                    else
                    {
                        while (normalData[i].High >= normalData[i - 1].Low)
                        {
                            ShiftDown(i);
                        }
                    }
                }
                else
                {
                    if (status[i].HigherHigh == 1)
                    {
                        ShiftUp(i);
                    }
                    else if(status[i].LowerLow == 1)
                    {
                        ShiftDown(i);
                    }
                }
            }
        }
        private void SettingDefaultCandle(int idx)
        {
            while (Math.Min(normalData[idx].Open, normalData[idx].Close) != Math.Min(normalData[idx - 1].Open, normalData[idx - 1].Close))
            {
                if (Math.Min(normalData[idx].Open, normalData[idx].Close) > Math.Min(normalData[idx - 1].Open, normalData[idx - 1].Close))
                {
                    ShiftDown(idx);
                }
                else
                {
                    ShiftUp(idx);
                }
            }
        }
        private void ShiftUp(int idx)
        {
            normalData[idx].Open += 1;
            normalData[idx].Low += 1;
            normalData[idx].Close += 1;
            normalData[idx].High += 1;
        }
        private void ShiftDown(int idx)
        {
            normalData[idx].Open -= 1;
            normalData[idx].Low -= 1;
            normalData[idx].Close -= 1;
            normalData[idx].High -= 1;
        }
    }
}
