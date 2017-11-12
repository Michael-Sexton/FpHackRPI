using System;
using System.Web;
using System.Web.UI;

namespace CrashNet
{
    
    public partial class Default : System.Web.UI.Page
    {
        public void button1Clicked(object sender, EventArgs args)
        {
			double[][] trainData = new double[19000][];

			double[][] testData = new double[1000][];
            //double[] bestWeights = nn.Train(trainData, numParticles, maxEpochs);
            //I hardcoded the best weights in there
            double[] bestWeights = {4.525, 6.347, 5.360, 1.163, -5.879, 1.178, 8.121, -1.156, 9.551, -4.526, 
                                    -4.162, -0.654, 2.653, -0.610, 9.643, -9.393, 7.247, 9.907, 3.544, -3.708, 6.338, 
                                    6.961, 9.838, -9.347, 3.999, 0.526, 8.680, 3.752, 0.936, -8.378, -6.258, -0.933, -4.057};
            button1.Text = "You clicked me";


        }

		static double[][] getData(double[][] trainData, String filepath)
		{
			string[] lines = System.IO.File.ReadAllLines(@filepath);
			Console.WriteLine(lines.Length);
			for (int i = 0; i < trainData.Length; i++)
			{
				//Takes out all the commas and separates gives the array temp each a different 
				String[] temp = lines[i].Split(',');
				//Console.WriteLine(temp[0] + "|" + temp[1] + "|" + temp[2]);

				Double tempBorough = 0;


				if (temp[1] == "BRONX")
				{
					tempBorough = .2;
				}
				else if (temp[1] == "BROOKLYN")
				{
					tempBorough = .4;
				}
				else if (temp[1] == "QUEENS")
				{
					tempBorough = .6;
				}
				else if (temp[1] == "STATEN ISLAND")
				{
					tempBorough = .8;
				}
				else if (temp[1] == "MANHATTAN")
				{
					tempBorough = 1;
				}

				double tempTime = 0.0;
				String[] time = temp[0].Split(':');
				//tempTime just combines the hour and the minutes
				tempTime = Convert.ToDouble(time[0]) + Convert.ToDouble(time[1]) / 60;

				Double numHurt = 0;

				//zeroes are indicators of more than one person getting injured
				if (Convert.ToDouble(temp[2]) >= 1)
				{
					numHurt = 0;
				}
				else
				{
					numHurt = 1;
				}


				trainData[i] = new double[] { tempBorough, tempTime, numHurt };

			}
			return trainData;
		}

		static void ShowVector(double[] vector, int valsPerRow, int decimals, bool newLine)
		{
			for (int i = 0; i < vector.Length; i++)
			{
				if (i % valsPerRow == 0) Console.WriteLine("");
				Console.Write(vector[i].ToString("F" + decimals).PadLeft(decimals + 4) + " ");
			}
			if (newLine == true) Console.WriteLine("");
		}

		static void ShowData(double[][] data, int numRows, int decimals, bool indices)
		{
			for (int i = 0; i < data.Length; i++)
			{
				if (indices == true)
				{
					Console.Write("[" + i.ToString().PadLeft(2) + "]    ");
				}
				for (int j = 0; j < data[i].Length; j++)
				{
					double v = data[i][j];
					if (v >= 0.0)
					{
						Console.Write(" "); // '+'
					}
					Console.Write(v.ToString("F" + decimals) + "     ");
				}
				Console.WriteLine("");
			}
			Console.WriteLine(". . .");
			int lastRow = data.Length - 1;
			if (indices == true)
			{
				Console.Write("[" + lastRow.ToString().PadLeft(2) + "]   ");
			}
			for (int j = 0; j < data[lastRow].Length; j++)
			{
				double v = data[lastRow][j];
				if (v >= 0.0)
				{
					Console.Write(" ");
				}
				Console.Write(v.ToString("F" + decimals) + "    ");
			}
			Console.Write("\n");
		}

		public class NeuralNetwork
		{
			private int numInput; // number of input nodes
			private int numHidden;
			private int numOutput;

			private double[] inputs;
			private double[][] ihWeights; // input-hidden
			private double[] hBiases;
			private double[] hOutputs;

			private double[][] hoWeights; // hidden-output
			private double[] oBiases;
			private double[] outputs;

			private Random rnd;

			public NeuralNetwork(int numInput, int numHidden, int numOutput)
			{
				this.numInput = numInput;
				this.numHidden = numHidden;
				this.numOutput = numOutput;
				this.inputs = new double[numInput];
				this.ihWeights = MakeMatrix(numInput, numHidden);
				this.hBiases = new double[numHidden];
				this.hOutputs = new double[numHidden];
				this.hoWeights = MakeMatrix(numHidden, numOutput);
				this.oBiases = new double[numOutput];
				this.outputs = new double[numOutput];
				this.rnd = new Random(0);
			} // ctor

			private static double[][] MakeMatrix(int rows, int cols) // helper for ctor
			{
				double[][] result = new double[rows][];
				for (int r = 0; r < result.Length; r++)
				{
					result[r] = new double[cols];
				}
				return result;
			}

			public void SetWeights(double[] weights)
			{
				// copy weights and biases in weight[] array to i-h weights,
				// i-h biases, h-o weights, h-o biases
				int numWeights = (numInput * numHidden) + (numHidden * numOutput) + numHidden + numOutput;
				if (weights.Length != numWeights)
					throw new Exception("Bad weights array length: ");

				int k = 0; // points into weights param
				for (int i = 0; i < numInput; i++)
				{
					for (int j = 0; j < numHidden; j++)
					{
						ihWeights[i][j] = weights[k++];
					}
				}
				for (int i = 0; i < numHidden; i++)
				{
					hBiases[i] = weights[k++];
				}
				for (int i = 0; i < numHidden; i++)
				{
					for (int j = 0; j < numOutput; j++)
					{
						hoWeights[i][j] = weights[k++];
					}
				}
				for (int i = 0; i < numOutput; i++)
				{
					oBiases[i] = weights[k++];
				}
			}

			public double[] ComputeOutputs(double[] xValues)
			{
				double[] hSums = new double[numHidden]; // hidden nodes sums scratch array
				double[] oSums = new double[numOutput]; // output nodes sums

				for (int i = 0; i < xValues.Length; i++) // copy x-values to inputs
				{
					this.inputs[i] = xValues[i];
				}

				for (int j = 0; j < numHidden; j++) // compute i-h sum of weights * inputs
				{
					for (int i = 0; i < numInput; i++)
					{
						hSums[j] += this.inputs[i] * this.ihWeights[i][j]; // note +=
					}
				}

				for (int i = 0; i < numHidden; i++) // add biases to input-to-hidden sums
				{
					hSums[i] += this.hBiases[i];
				}

				for (int i = 0; i < numHidden; i++) // apply activation of the hyperbolic tangent function
				{
					this.hOutputs[i] = HyperTan(hSums[i]);
				}

				for (int j = 0; j < numOutput; j++) // compute h-o sum of weights * hOutputs
				{
					for (int i = 0; i < numHidden; i++)
					{
						oSums[j] += hOutputs[i] * hoWeights[i][j];
					}
				}

				for (int i = 0; i < numOutput; i++) // add biases to input-to-hidden sums
				{
					oSums[i] += oBiases[i];
				}

				double[] softOut = Softmax(oSums); // all outputs at once for efficiency 
				Array.Copy(softOut, outputs, softOut.Length);

				double[] retResult = new double[numOutput];
				Array.Copy(this.outputs, retResult, retResult.Length);
				return retResult;
			}

			private static double HyperTan(double x)
			{
				if (x < -20.0)
				{
					return -1.0; // approximation is correct to 30 decimals
				}
				else if (x > 20.0)
				{
					return 1.0;
				}
				else
				{
					return Math.Tanh(x);
				}
			}

			private static double[] Softmax(double[] oSums)
			{
				// does all output nodes at once so scale doesn't have to be re-computed each time
				// determine max output-sum
				double max = oSums[0];
				for (int i = 0; i < oSums.Length; i++)
				{
					if (oSums[i] > max)
					{
						max = oSums[i];
					}
				}

				// determine scaling factor -- sum of exp(each val - max)
				double scale = 0.0;
				for (int i = 0; i < oSums.Length; i++)
				{
					scale += Math.Exp(oSums[i] - max);
				}

				double[] result = new double[oSums.Length];
				for (int i = 0; i < oSums.Length; i++)
				{
					result[i] = Math.Exp(oSums[i] - max) / scale;
				}

				return result; // now scaled so that xi sum to 1.0
			}

			public double[] Train(double[][] trainData, int numParticles, int maxEpochs)
			{
				int numWeights = (this.numInput * this.numHidden) + this.numHidden + (this.numHidden * this.numOutput) + this.numOutput;

				// use PSO to seek best weights
				int epoch = 0;
				double minX = -10.0; // for each weight. assumes data is normalized or "nice"
				double maxX = 10.0;
				double w = 0.729; // inertia weight
				double c1 = 1.49445; // cognitive weight
				double c2 = 1.49445; // social weight
				double r1, r2; // cognitive and social randomizations

				Particle[] swarm = new Particle[numParticles];
				// best solution found by any particle in the swarm
				double[] bestGlobalPosition = new double[numWeights];
				double bestGlobalError = double.MaxValue; // smaller values better

				// initialize each Particle in the sawm with random positions and velocities
				double lo = 0.1 * minX;
				double hi = 0.1 * maxX;

				for (int i = 0; i < swarm.Length; i++)
				{
					double[] randomPosition = new double[numWeights];
					for (int j = 0; j < randomPosition.Length; j++)
					{
						randomPosition[j] = (maxX - minX) * rnd.NextDouble() + minX;
					}

					double error = MeanSquaredError(trainData, randomPosition);
					double[] randomVelocity = new double[numWeights];

					for (int j = 0; j < randomVelocity.Length; j++)
					{
						randomVelocity[j] = (hi - lo) * rnd.NextDouble() + lo;
					}

					swarm[i] = new Particle(randomPosition, error, randomVelocity, randomPosition, error);

					// does current Particle have global best position/solution?

					if (swarm[i].error < bestGlobalError)
					{
						bestGlobalError = swarm[i].error;
						swarm[i].position.CopyTo(bestGlobalPosition, 0);
					}
				}

				//main PSO algorithm
				int[] sequence = new int[numParticles]; // process particles in random order
				for (int i = 0; i < sequence.Length; i++)
				{
					sequence[i] = i;
				}

				while (epoch < maxEpochs)
				{
					double[] newVelocity = new double[numWeights]; // step 1
					double[] newPosition = new double[numWeights]; // step 2
					double newError;

					Shuffle(sequence);

					for (int pi = 0; pi < swarm.Length; pi++) // each Particle (index)
					{
						int i = sequence[pi];
						Particle currP = swarm[i]; // for coding convenience

						//  1. compute new velocity
						for (int j = 0; j < currP.velocity.Length; j++) // each value of the velocity
						{
							r1 = rnd.NextDouble();
							r2 = rnd.NextDouble();

							// velocity depends on old velocity, best position of particle, and best position of any particle
							newVelocity[j] = (w * currP.velocity[j]) + (c1 * r1 * (currP.bestPosition[j] - currP.position[j])) + (c2 * r2 * (bestGlobalPosition[j] - currP.position[j]));
						}
						newVelocity.CopyTo(currP.velocity, 0);

						// 2. use new velocity to compute new position
						for (int j = 0; j < currP.velocity.Length; j++)
						{
							newPosition[j] = currP.position[j] + newVelocity[j];
							if (newPosition[j] < minX) // kep in Range
							{
								newPosition[j] = minX;
							}
							else if (newPosition[j] > maxX)
							{
								newPosition[j] = maxX;
							}
						}
						newPosition.CopyTo(currP.position, 0);

						// 3. compute error of new position
						newError = MeanSquaredError(trainData, newPosition);
						currP.error = newError;

						if (newError < currP.bestError) // new particle best?
						{
							newPosition.CopyTo(currP.bestPosition, 0);
							currP.error = newError;
						}

						if (newError < bestGlobalError) // new global best?
						{
							newPosition.CopyTo(bestGlobalPosition, 0);
							bestGlobalError = newError;
						}
					} // each Particle
					epoch++;
				} // while

				SetWeights(bestGlobalPosition); // best position is a set of weights
				double[] retResult = new double[numWeights];
				Array.Copy(bestGlobalPosition, retResult, retResult.Length);
				return retResult;
			} // Train

			private void Shuffle(int[] sequence)
			{
				for (int i = 0; i < sequence.Length; i++)
				{
					int ri = rnd.Next(i, sequence.Length);
					int tmp = sequence[ri];
					sequence[ri] = sequence[i];
					sequence[i] = tmp;
				}
			}

			private double MeanSquaredError(double[][] trainData, double[] weights)
			{
				this.SetWeights(weights); // copy the weights to evaluate in

				double[] xValues = new double[numInput]; // inputs
				double[] tValues = new double[numOutput]; // targets
				double sumSquaredError = 0.0;

				for (int i = 0; i < trainData.Length; i++) // walk through each training item
				{
					// the following assumes data has all x-values first, folowed by y-values!
					Array.Copy(trainData[i], xValues, numInput); // extract inputs
																 //Console.WriteLine(i.ToString() + numInput.ToString() + tValues.ToString() + numOutput.ToString());
					Array.Copy(trainData[i], numInput, tValues, 0, numOutput); // extract targets
					double[] yValues = this.ComputeOutputs(xValues);

					for (int j = 0; j < yValues.Length; j++)
					{
						sumSquaredError += ((yValues[j] - tValues[j]) * (yValues[j] - tValues[j]));
					}
				}
				return sumSquaredError / trainData.Length;
			}

			public double Accuracy(double[][] testData)
			{
				// percentage correct using winer-takes all
				int numCorrect = 0;
				int numWrong = 0;
				double[] xValues = new double[numInput]; // inputs
				double[] tValues = new double[numOutput]; // targets
				double[] yValues; // computed Y

				for (int i = 0; i < testData.Length; i++)
				{
					Array.Copy(testData[i], xValues, numInput); // parse test data
					Array.Copy(testData[i], numInput, tValues, 0, numOutput);
					yValues = this.ComputeOutputs(xValues);

					//This simply will check for the highest y-value
					int maxIndex = MaxIndex(yValues);

					if (tValues[maxIndex] == 1.0)
					{
						numCorrect++;
					}
					else
					{
						numWrong++;
					}
				}
				return (numCorrect * 100.0) / (numCorrect + numWrong);
			}

			private static int MaxIndex(double[] vector) // helper for Accuracy()
			{
				// index of largest value
				int bigIndex = 0;
				double biggestVal = vector[0];
				for (int i = 0; i < vector.Length; i++)
				{
					if (vector[i] > biggestVal)
					{
						biggestVal = vector[i];
						bigIndex = i;
					}
				}
				return bigIndex;
			}

			private class Particle
			{
				public double[] position; // equivalent to NN weights
				public double error; // measure of fitness
				public double[] velocity;

				public double[] bestPosition; // best position found by this Particle
				public double bestError;

				public Particle(double[] position, double error, double[] velocity, double[] bestPosition, double bestError)
				{
					this.position = new double[position.Length];
					position.CopyTo(this.position, 0);
					this.error = error;
					this.velocity = new double[velocity.Length];
					velocity.CopyTo(this.velocity, 0);
					this.bestPosition = new double[bestPosition.Length];
					bestPosition.CopyTo(this.bestPosition, 0);
					this.bestError = bestError;
				}
			}

		}
	    }
    }

