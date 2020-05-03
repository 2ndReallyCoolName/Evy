using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Accord.Math;
using NAudio.Wave;

namespace AudioAnalysis
{
    public class Fourier
    {

        public static double[] Transform(short[] buffer)
        {
            Complex[] fftComplex = new Complex[buffer.Length];

            for (int i = 0; i < buffer.Length; i++)
            {
                fftComplex[i] = new Complex(buffer[i], 0.0);
            }

            int sz = AudioAnalysis.Fourier.nearest_power_2(fftComplex.Length);

            Complex[] data = new Complex[sz];

            Array.Copy(fftComplex, data, sz);

            double[] result = new double[sz];

            FourierTransform.FFT(data, FourierTransform.Direction.Forward);

            for (int i = 0; i < data.Length; i++)
                result[i] = data[i].Magnitude;
            return result;
        }


        public static short[] Sin(int[] fs, float time, int sampling_rate, double[] amps)
        {
            short[] v = new short[(int)(time * sampling_rate)];
            float dt = 1.0f / sampling_rate;
            float t = 0;
            for (var k = 0; k < v.Length; k += 1)
            {
                short val = 0;
                for (int j = 0; j < fs.Length; j++)
                {
                    val += Convert.ToInt16(amps[j] * Math.Sin(2 * Math.PI * fs[j] * t));
                }
                v[k] = val;
                t += dt;
            }
            return v;
        }

        public static int nearest_power_2(int x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);

            x -= (x >> 1);
            if (Math.Log2(x) > 14)
            {
                return (int)Math.Pow(2, 14);
            }

            return x;
        }

        public static double[] FirstHalf(double[] data)
        {
            int s = (int)(data.Length / 2);
            double[] d = new double[s];
            Array.Copy(data, d, s);
            return d;
        }

        public static double Frequency(double sample_rate, double size, double bin)
        {
            return bin * sample_rate / size;
        }

        public static double[][] FFT(short[] buffer, int k, int sample_rate)
        {
            double[] data = AudioAnalysis.Fourier.FirstHalf(AudioAnalysis.Fourier.Transform(buffer));

            var m = data.Select((x, i) => new KeyValuePair<double, int>(x, i))
                .OrderByDescending(x => x.Key).ToArray();

            m = m.Where(x => ((AudioAnalysis.Fourier.Frequency(sample_rate, 2 * data.Length, x.Value) >= 20) &&
            (AudioAnalysis.Fourier.Frequency(sample_rate, 2 * data.Length, x.Value) <= 20000))).Take(k).ToArray();

            double[] vals = m.Select(x => x.Key).ToArray();
            double[] idxs = m.Select(x => AudioAnalysis.Fourier.Frequency(sample_rate, 2 * data.Length, x.Value)).ToArray();
            return new double[][] { vals, idxs };
        }


    }
}
