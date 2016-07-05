﻿using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concentus
{
    /// <summary>
    /// multi-layer perceptron processor
    /// </summary>
    internal static class mlp
    {
        private const int MAX_NEURONS = 100;

        internal static float tansig_approx(float x)
        {
            int i;
            float y, dy;
            float sign = 1;
            /* Tests are reversed to catch NaNs */
            if (!(x < 8))
                return 1;
            if (!(x > -8))
                return -1;
            if (x < 0)
            {
                x = -x;
                sign = -1;
            }
            i = (int)Math.Floor(.5f + 25 * x);
            x -= .04f * i;
            y = Tables.tansig_table[i];
            dy = 1 - y * y;
            y = y + x * dy * (1 - y * x);
            return sign * y;
        }

        internal static void mlp_process(MLP m, Pointer<float> input, Pointer<float> output)
        {
            int j;
            float[] hidden = new float[MAX_NEURONS];
            float[] W = m.weights;
            int W_ptr = 0;

            /* Copy to tmp_in */

            for (j = 0; j < m.topo[1]; j++)
            {
                int k;
                float sum = W[W_ptr];
                W_ptr++;
                for (k = 0; k < m.topo[0]; k++)
                {
                    sum = sum + input[k] * W[W_ptr];
                    W_ptr++;
                }
                hidden[j] = tansig_approx(sum);
            }

            for (j = 0; j < m.topo[2]; j++)
            {
                int k;
                float sum = W[W_ptr];
                W_ptr++;
                for (k = 0; k < m.topo[1]; k++)
                {
                    sum = sum + hidden[k] * W[W_ptr];
                    W_ptr++;
                }
                output[j] = tansig_approx(sum);
            }
        }
    }
}