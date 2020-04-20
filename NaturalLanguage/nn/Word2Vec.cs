using NumSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tensorflow;
using static Tensorflow.Binding;
using Accord.Math;

namespace NaturalLanguage.NN
{
    public class Word2Vec : AbstractNN, INN
    {
        Dictionary<string, int> word2id = new Dictionary<string, int>();
        Dictionary<string, float[]> wordsvecs = new Dictionary<string, float[]>();

        float[,] diag = new float[50, 50];

        readonly int output_size = 10;

        readonly string graph_path = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())), $"Eevee\\Eevee\\bin\\graph2");

        //Tensors
        Tensor X = null;
        Tensor Y = null;
        Tensor loss_op = null;
        Operation train_op = null;
        Tensor embedding = null;
        Tensor cosine_sim_op = null;

        Graph graph = null;
        Accord.Math.Decompositions.SingularValueDecomposition M = null;

        public Word2Vec()
        {
            BuildGraph();

            PrepareData();
            
        }


        public Config InitConfig()
            => Config = new Config
            {
                Name = "Word2Vec",
                Enabled = true,
                IsImportingGraph = true
            };


        public override void BuildGraph()
        {
            graph = tf.Graph().as_default();

            tf.train.import_meta_graph(Path.Combine(graph_path, "my-model2.meta"));

            // Input data
            X = graph.OperationByName("Placeholder");
            // Input label
            Y = graph.OperationByName("Placeholder_1");

            embedding = graph.OperationByName("embedding_lookup");

            // Compute the average NCE loss for the batch
            loss_op = graph.OperationByName("Mean");
            // Define the optimizer
            train_op = graph.OperationByName("GradientDescent");
            cosine_sim_op = graph.OperationByName("MatMul_1");
        }

        public override void Train()
        {

        }

        public bool Run()
        {
            return true;
        }


        public override float[] Predict(string word)
        {
            if (wordsvecs.ContainsKey(word)){
                return wordsvecs[word];
            }else 
            
            if (word2id.ContainsKey(word)) { 


                using (var sess = tf.Session(graph))
                {


                    tf.train.Saver().restore(sess, tf.train.latest_checkpoint(graph_path));

                    int[] s = { word2id[word] };

                    var emb = sess.run(embedding, (X, s));

                    var vector = NaturalLanguage.vector.VectorSpace.Normalize(emb[0].Data<float>().ToArray());

                    return Matrix.Dot(vector, diag).Take(output_size).ToArray();

                    //return NaturalLanguage.vector.VectorSpace.Normalize(emb[0].Data<float>().ToArray());
                }
            }
            else
            {
                return new float[output_size];
            }

        }

        public override float[] PredictText(string text)
        {
            string[] words = Text.RemoveStopWords.RemoveWords(text.Trim().ToLower());

            if(words == null)
            {
                return new float[output_size];
            }

            float[][] vectors = new float[words.Length][];

            for (int i = 0; i < words.Length; i++)
            {
                vectors[i] = Predict(words[i]);
            }

            // Parallel.ForEach(words, word =>
            //{
            //    vector = NaturalLanguage.vector.VectorSpace.Add(vector, model.Predict(word));
            //});

            return vector.VectorSpace.Normalize(vector.VectorSpace.Add(vectors));
        }

        public override void PrepareData()
        {
            var lines = File.ReadAllLines(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "words.txt"), Encoding.UTF8);

            foreach (var line in lines)
            {
                var ls = line.Split(' ');
                word2id.Add(ls[0], Int32.Parse(ls[1]));
            }


            var vecs = File.ReadAllLines(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wordvecs2.txt"), Encoding.UTF8);

            var l = vecs.Length;

            for (int i = 0; i < l; i++)
            {
                var sl = vecs[i].Split(':');
                wordsvecs.Add(sl[0], NaturalLanguage.vector.VectorSpace.ToArray(sl[1]));
            }


            var diags = File.ReadAllText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "diag.txt"), Encoding.UTF8).Split(',');

            for (int i = 0; i < diags.Length; i++)
                diag[i, i] = float.Parse(diags[i]);
        }

        public override int GetOutputSize()
        {
            return output_size;
        }
    }
}