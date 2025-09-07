using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using Unity.VisualScripting;

namespace EXAMPLE
{
    public class Main : MonoBehaviour
    {
        public TextMeshProUGUI ResultText;
        public TextMeshProUGUI ButtonText;
        public int NumIterations = 10000;
        public int ArraySize = 1000;

        bool m_runTest = false;

        int[] randomPaths100;
        int[] upperHalf100;
        int[] lowerHalf100;
        int[] worstCase100;

        int[] randomPaths50;
        int[] upperHalf50;
        int[] lowerHalf50;
        int[] worstCase50;

        int[] randomPaths30;
        int[] upperHalf30;
        int[] lowerHalf30;
        int[] worstCase30;

        int[] randomPaths10;
        int[] upperHalf10;
        int[] lowerHalf10;
        int[] worstCase10;

        double[] m_time = new double[32];

        private void Awake()
        {
            ResultText.text = "Switch Test\n";

            randomPaths100 = new int[ArraySize];
            upperHalf100 = new int[ArraySize];
            lowerHalf100 = new int[ArraySize];
            worstCase100 = new int[ArraySize];

            randomPaths50 = new int[ArraySize];
            upperHalf50 = new int[ArraySize];
            lowerHalf50 = new int[ArraySize];
            worstCase50 = new int[ArraySize];

            randomPaths30 = new int[ArraySize];
            upperHalf30 = new int[ArraySize];
            lowerHalf30 = new int[ArraySize];
            worstCase30 = new int[ArraySize];

            randomPaths10 = new int[ArraySize];
            upperHalf10 = new int[ArraySize];
            lowerHalf10 = new int[ArraySize];
            worstCase10 = new int[ArraySize];

#if UNITY_EDITOR
            int size = 50;
            string switchString = "int switchTest" + size + "(int a)\n{\nswitch(a) {\n";
            for (int i = 0; i < size; i++)
            {
                switchString += "case " + i + ": return " + i + ";\n";
            }
            switchString += "default: return 0;\n}\n}";

            File.WriteAllText("switchTest" + size + ".txt", switchString);

            string ifString = "int ifTest" + size + "(int a)\n{\nif(a == 0) return 0;\n";
            for (int i = 1; i < size; i++)
            {
                ifString += "else if(a == " + i + ") return " + i + ";\n";
            }
            ifString += "return 0;\n}";

            File.WriteAllText("ifTest" + size + ".txt", ifString);
#endif
        }

        public void RunTest()
        {
            m_runTest = true;
            ButtonText.text = "Running...";
        }

        private void Update()
        {
            if (m_runTest)
            {
                double time = 0.0d;

                int size = 0;
                for (int i = 0; i < ArraySize; i++)
                {
                    size = 100;
                    randomPaths100[i] = (int)(Random.value * size);
                    lowerHalf100[i] = (int)(Random.value * size / 2);
                    upperHalf100[i] = (int)(Random.value * size / 2) + (size / 2);
                    worstCase100[i] = i % 2 + (size - 2); // alternating to defeat branch prediction

                    size = 50;
                    randomPaths50[i] = (int)(Random.value * size);
                    lowerHalf50[i] = (int)(Random.value * size / 2);
                    upperHalf50[i] = (int)(Random.value * size / 2) + (size / 2);
                    worstCase50[i] = i % 2 + (size - 2); // alternating to defeat branch prediction

                    size = 30;
                    randomPaths30[i] = (int)(Random.value * size);
                    lowerHalf30[i] = (int)(Random.value * size / 2);
                    upperHalf30[i] = (int)(Random.value * size / 2) + (size / 2);
                    worstCase30[i] = i % 2 + (size - 2); // alternating to defeat branch prediction

                    size = 10;
                    randomPaths10[i] = (int)(Random.value * size);
                    lowerHalf10[i] = (int)(Random.value * size / 2);
                    upperHalf10[i] = (int)(Random.value * size / 2) + (size / 2);
                    worstCase10[i] = i % 2 + (size - 2); // alternating to defeat branch prediction
                }

                for (int i = 0; i < m_time.Length; i++)
                    m_time[i] = 0.0d;

                int count = 0;

                for (int t = 0; t < NumIterations; t++)
                {

                    count = 0;

                    runSwitchTest10(ref count, randomPaths10, lowerHalf10, upperHalf10, worstCase10);

                    runSwitchTest30(ref count, randomPaths30, lowerHalf30, upperHalf30, worstCase30);

                    runSwitchTest50(ref count, randomPaths50, lowerHalf50, upperHalf50, worstCase50);

                    runSwitchTest100(ref count, randomPaths100, lowerHalf100, upperHalf100, worstCase100);
                }

                count = 0;
                ResultText.text = "";
                ResultText.text += "Test finished\n";
                ResultText.text += "Num Iterations " + NumIterations + "\n";
                ResultText.text += "ArraySize " + ArraySize + "\n";
                count = printSwitchTest(count, "10");
                count = printSwitchTest(count, "30");
                count = printSwitchTest(count, "50");
                count = printSwitchTest(count, "100");
                m_runTest = false;
                ButtonText.text = "Rerun Test";
            }
        }

        private int printSwitchTest(int count, string size)
        {
            ResultText.text += "\n";
            ResultText.text += size + " Random Switch test " + m_time[count++].ToString("G4") + whichFaster(m_time[count - 1], m_time[count]) + "\n";
            ResultText.text += size + " Random If test " + m_time[count++].ToString("G4") + whichFaster(m_time[count - 1], m_time[count - 2]) + "\n";
            ResultText.text += size + " Lower Half Switch test " + m_time[count++].ToString("G4") + whichFaster(m_time[count - 1], m_time[count]) + "\n";
            ResultText.text += size + " Lower Half If test " + m_time[count++].ToString("G4") + whichFaster(m_time[count - 1], m_time[count - 2]) + "\n";
            ResultText.text += size + " Upper Half Switch test " + m_time[count++].ToString("G4") + whichFaster(m_time[count - 1], m_time[count]) + "\n";
            ResultText.text += size + " Upper Half If test " + m_time[count++].ToString("G4") + whichFaster(m_time[count - 1], m_time[count - 2]) + "\n";
            ResultText.text += size + " Worst Case Switch test " + m_time[count++].ToString("G4") + whichFaster(m_time[count - 1], m_time[count]) + "\n";
            ResultText.text += size + " Worst Case If test " + m_time[count++].ToString("G4") + whichFaster(m_time[count - 1], m_time[count - 2]) + "\n";
            return count;
        }

        private string whichFaster(double a, double b)
        {
            return " " + ((a > b) ? "" : (b / a).ToString("G2") + "x faster");
        }

        private int runSwitchTest10(ref int count, int[] randomPath, int[] lowerHalf, int[] upperHalf, int[] worstCase)
        {
            int a = 0;
            double time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest10(randomPath[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest10(randomPath[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest10(lowerHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest10(lowerHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest10(upperHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest10(upperHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest10(worstCase[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest10(worstCase[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            return a;
        }

        private int runSwitchTest30(ref int count, int[] randomPath, int[] lowerHalf, int[] upperHalf, int[] worstCase)
        {
            int a = 0;
            double time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest30(randomPath[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest30(randomPath[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest30(lowerHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest30(lowerHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest30(upperHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest30(upperHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest30(worstCase[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest30(worstCase[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            return a;
        }

        private int runSwitchTest50(ref int count, int[] randomPath, int[] lowerHalf, int[] upperHalf, int[] worstCase)
        {
            int a = 0;
            double time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i <ArraySize; i++)
                a += switchTest50(randomPath[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest50(randomPath[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest50(lowerHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest50(lowerHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest50(upperHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest50(upperHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest50(worstCase[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest50(worstCase[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            return a;
        }

        private int runSwitchTest100(ref int count, int[] randomPath, int[] lowerHalf, int[] upperHalf, int[] worstCase)
        {
            int a = 0;
            double time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest100(randomPath[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest100(randomPath[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest100(lowerHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest100(lowerHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest100(upperHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest100(upperHalf[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += switchTest100(worstCase[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            time = Time.realtimeSinceStartupAsDouble;
            for (int i = 0; i < ArraySize; i++)
                a += ifTest100(worstCase[i]);
            m_time[count++] += Time.realtimeSinceStartupAsDouble - time;

            return a;
        }

        int switchTest10(int a)
        {
            switch (a)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 11;
                case 12: return 12;
                case 13: return 13;
                case 14: return 14;
                case 15: return 15;
                case 16: return 16;
                case 17: return 17;
                case 18: return 18;
                case 19: return 19;
                default: return 0;
            }
        }

        int ifTest10(int a)
        {
            if (a == 0) return 0;
            else if (a == 1) return 1;
            else if (a == 2) return 2;
            else if (a == 3) return 3;
            else if (a == 4) return 4;
            else if (a == 5) return 5;
            else if (a == 6) return 6;
            else if (a == 7) return 7;
            else if (a == 8) return 8;
            else if (a == 9) return 9;
            else if (a == 10) return 10;
            else if (a == 11) return 11;
            else if (a == 12) return 12;
            else if (a == 13) return 13;
            else if (a == 14) return 14;
            else if (a == 15) return 15;
            else if (a == 16) return 16;
            else if (a == 17) return 17;
            else if (a == 18) return 18;
            else if (a == 19) return 19;
            return 0;
        }

        int switchTest20(int a)
        {
            switch (a)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 11;
                case 12: return 12;
                case 13: return 13;
                case 14: return 14;
                case 15: return 15;
                case 16: return 16;
                case 17: return 17;
                case 18: return 18;
                case 19: return 19;
                default: return 0;
            }
        }

        int ifTest20(int a)
        {
            if (a == 0) return 0;
            else if (a == 1) return 1;
            else if (a == 2) return 2;
            else if (a == 3) return 3;
            else if (a == 4) return 4;
            else if (a == 5) return 5;
            else if (a == 6) return 6;
            else if (a == 7) return 7;
            else if (a == 8) return 8;
            else if (a == 9) return 9;
            else if (a == 10) return 10;
            else if (a == 11) return 11;
            else if (a == 12) return 12;
            else if (a == 13) return 13;
            else if (a == 14) return 14;
            else if (a == 15) return 15;
            else if (a == 16) return 16;
            else if (a == 17) return 17;
            else if (a == 18) return 18;
            else if (a == 19) return 19;
            return 0;
        }

        int switchTest30(int a)
        {
            switch (a)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 11;
                case 12: return 12;
                case 13: return 13;
                case 14: return 14;
                case 15: return 15;
                case 16: return 16;
                case 17: return 17;
                case 18: return 18;
                case 19: return 19;
                case 20: return 20;
                case 21: return 21;
                case 22: return 22;
                case 23: return 23;
                case 24: return 24;
                case 25: return 25;
                case 26: return 26;
                case 27: return 27;
                case 28: return 28;
                case 29: return 29;
                default: return 0;
            }
        }

        int ifTest30(int a)
        {
            if (a == 0) return 0;
            else if (a == 1) return 1;
            else if (a == 2) return 2;
            else if (a == 3) return 3;
            else if (a == 4) return 4;
            else if (a == 5) return 5;
            else if (a == 6) return 6;
            else if (a == 7) return 7;
            else if (a == 8) return 8;
            else if (a == 9) return 9;
            else if (a == 10) return 10;
            else if (a == 11) return 11;
            else if (a == 12) return 12;
            else if (a == 13) return 13;
            else if (a == 14) return 14;
            else if (a == 15) return 15;
            else if (a == 16) return 16;
            else if (a == 17) return 17;
            else if (a == 18) return 18;
            else if (a == 19) return 19;
            else if (a == 20) return 20;
            else if (a == 21) return 21;
            else if (a == 22) return 22;
            else if (a == 23) return 23;
            else if (a == 24) return 24;
            else if (a == 25) return 25;
            else if (a == 26) return 26;
            else if (a == 27) return 27;
            else if (a == 28) return 28;
            else if (a == 29) return 29;
            return 0;
        }

        int ifTest50(int a)
        {
            if (a == 0) return 0;
            else if (a == 1) return 1;
            else if (a == 2) return 2;
            else if (a == 3) return 3;
            else if (a == 4) return 4;
            else if (a == 5) return 5;
            else if (a == 6) return 6;
            else if (a == 7) return 7;
            else if (a == 8) return 8;
            else if (a == 9) return 9;
            else if (a == 10) return 10;
            else if (a == 11) return 11;
            else if (a == 12) return 12;
            else if (a == 13) return 13;
            else if (a == 14) return 14;
            else if (a == 15) return 15;
            else if (a == 16) return 16;
            else if (a == 17) return 17;
            else if (a == 18) return 18;
            else if (a == 19) return 19;
            else if (a == 20) return 20;
            else if (a == 21) return 21;
            else if (a == 22) return 22;
            else if (a == 23) return 23;
            else if (a == 24) return 24;
            else if (a == 25) return 25;
            else if (a == 26) return 26;
            else if (a == 27) return 27;
            else if (a == 28) return 28;
            else if (a == 29) return 29;
            else if (a == 30) return 30;
            else if (a == 31) return 31;
            else if (a == 32) return 32;
            else if (a == 33) return 33;
            else if (a == 34) return 34;
            else if (a == 35) return 35;
            else if (a == 36) return 36;
            else if (a == 37) return 37;
            else if (a == 38) return 38;
            else if (a == 39) return 39;
            else if (a == 40) return 40;
            else if (a == 41) return 41;
            else if (a == 42) return 42;
            else if (a == 43) return 43;
            else if (a == 44) return 44;
            else if (a == 45) return 45;
            else if (a == 46) return 46;
            else if (a == 47) return 47;
            else if (a == 48) return 48;
            else if (a == 49) return 49;
            return 0;
        }

        int switchTest50(int a)
        {
            switch (a)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 11;
                case 12: return 12;
                case 13: return 13;
                case 14: return 14;
                case 15: return 15;
                case 16: return 16;
                case 17: return 17;
                case 18: return 18;
                case 19: return 19;
                case 20: return 20;
                case 21: return 21;
                case 22: return 22;
                case 23: return 23;
                case 24: return 24;
                case 25: return 25;
                case 26: return 26;
                case 27: return 27;
                case 28: return 28;
                case 29: return 29;
                case 30: return 30;
                case 31: return 31;
                case 32: return 32;
                case 33: return 33;
                case 34: return 34;
                case 35: return 35;
                case 36: return 36;
                case 37: return 37;
                case 38: return 38;
                case 39: return 39;
                case 40: return 40;
                case 41: return 41;
                case 42: return 42;
                case 43: return 43;
                case 44: return 44;
                case 45: return 45;
                case 46: return 46;
                case 47: return 47;
                case 48: return 48;
                case 49: return 49;
                default: return 0;
            }
        }

        int ifTest100(int a)
        {
            if (a == 0) return 0;
            else if (a == 1) return 1;
            else if (a == 2) return 2;
            else if (a == 3) return 3;
            else if (a == 4) return 4;
            else if (a == 5) return 5;
            else if (a == 6) return 6;
            else if (a == 7) return 7;
            else if (a == 8) return 8;
            else if (a == 9) return 9;
            else if (a == 10) return 10;
            else if (a == 11) return 11;
            else if (a == 12) return 12;
            else if (a == 13) return 13;
            else if (a == 14) return 14;
            else if (a == 15) return 15;
            else if (a == 16) return 16;
            else if (a == 17) return 17;
            else if (a == 18) return 18;
            else if (a == 19) return 19;
            else if (a == 20) return 20;
            else if (a == 21) return 21;
            else if (a == 22) return 22;
            else if (a == 23) return 23;
            else if (a == 24) return 24;
            else if (a == 25) return 25;
            else if (a == 26) return 26;
            else if (a == 27) return 27;
            else if (a == 28) return 28;
            else if (a == 29) return 29;
            else if (a == 30) return 30;
            else if (a == 31) return 31;
            else if (a == 32) return 32;
            else if (a == 33) return 33;
            else if (a == 34) return 34;
            else if (a == 35) return 35;
            else if (a == 36) return 36;
            else if (a == 37) return 37;
            else if (a == 38) return 38;
            else if (a == 39) return 39;
            else if (a == 40) return 40;
            else if (a == 41) return 41;
            else if (a == 42) return 42;
            else if (a == 43) return 43;
            else if (a == 44) return 44;
            else if (a == 45) return 45;
            else if (a == 46) return 46;
            else if (a == 47) return 47;
            else if (a == 48) return 48;
            else if (a == 49) return 49;
            else if (a == 50) return 50;
            else if (a == 51) return 51;
            else if (a == 52) return 52;
            else if (a == 53) return 53;
            else if (a == 54) return 54;
            else if (a == 55) return 55;
            else if (a == 56) return 56;
            else if (a == 57) return 57;
            else if (a == 58) return 58;
            else if (a == 59) return 59;
            else if (a == 60) return 60;
            else if (a == 61) return 61;
            else if (a == 62) return 62;
            else if (a == 63) return 63;
            else if (a == 64) return 64;
            else if (a == 65) return 65;
            else if (a == 66) return 66;
            else if (a == 67) return 67;
            else if (a == 68) return 68;
            else if (a == 69) return 69;
            else if (a == 70) return 70;
            else if (a == 71) return 71;
            else if (a == 72) return 72;
            else if (a == 73) return 73;
            else if (a == 74) return 74;
            else if (a == 75) return 75;
            else if (a == 76) return 76;
            else if (a == 77) return 77;
            else if (a == 78) return 78;
            else if (a == 79) return 79;
            else if (a == 80) return 80;
            else if (a == 81) return 81;
            else if (a == 82) return 82;
            else if (a == 83) return 83;
            else if (a == 84) return 84;
            else if (a == 85) return 85;
            else if (a == 86) return 86;
            else if (a == 87) return 87;
            else if (a == 88) return 88;
            else if (a == 89) return 89;
            else if (a == 90) return 90;
            else if (a == 91) return 91;
            else if (a == 92) return 92;
            else if (a == 93) return 93;
            else if (a == 94) return 94;
            else if (a == 95) return 95;
            else if (a == 96) return 96;
            else if (a == 97) return 97;
            else if (a == 98) return 98;
            else if (a == 99) return 99;
            return 0;
        }

        int switchTest100(int a)
        {
            switch (a)
            {
                case 0: return 0;
                case 1: return 1;
                case 2: return 2;
                case 3: return 3;
                case 4: return 4;
                case 5: return 5;
                case 6: return 6;
                case 7: return 7;
                case 8: return 8;
                case 9: return 9;
                case 10: return 10;
                case 11: return 11;
                case 12: return 12;
                case 13: return 13;
                case 14: return 14;
                case 15: return 15;
                case 16: return 16;
                case 17: return 17;
                case 18: return 18;
                case 19: return 19;
                case 20: return 20;
                case 21: return 21;
                case 22: return 22;
                case 23: return 23;
                case 24: return 24;
                case 25: return 25;
                case 26: return 26;
                case 27: return 27;
                case 28: return 28;
                case 29: return 29;
                case 30: return 30;
                case 31: return 31;
                case 32: return 32;
                case 33: return 33;
                case 34: return 34;
                case 35: return 35;
                case 36: return 36;
                case 37: return 37;
                case 38: return 38;
                case 39: return 39;
                case 40: return 40;
                case 41: return 41;
                case 42: return 42;
                case 43: return 43;
                case 44: return 44;
                case 45: return 45;
                case 46: return 46;
                case 47: return 47;
                case 48: return 48;
                case 49: return 49;
                case 50: return 50;
                case 51: return 51;
                case 52: return 52;
                case 53: return 53;
                case 54: return 54;
                case 55: return 55;
                case 56: return 56;
                case 57: return 57;
                case 58: return 58;
                case 59: return 59;
                case 60: return 60;
                case 61: return 61;
                case 62: return 62;
                case 63: return 63;
                case 64: return 64;
                case 65: return 65;
                case 66: return 66;
                case 67: return 67;
                case 68: return 68;
                case 69: return 69;
                case 70: return 70;
                case 71: return 71;
                case 72: return 72;
                case 73: return 73;
                case 74: return 74;
                case 75: return 75;
                case 76: return 76;
                case 77: return 77;
                case 78: return 78;
                case 79: return 79;
                case 80: return 80;
                case 81: return 81;
                case 82: return 82;
                case 83: return 83;
                case 84: return 84;
                case 85: return 85;
                case 86: return 86;
                case 87: return 87;
                case 88: return 88;
                case 89: return 89;
                case 90: return 90;
                case 91: return 91;
                case 92: return 92;
                case 93: return 93;
                case 94: return 94;
                case 95: return 95;
                case 96: return 96;
                case 97: return 97;
                case 98: return 98;
                case 99: return 99;
                default: return 0;
            }
        }

    }
}