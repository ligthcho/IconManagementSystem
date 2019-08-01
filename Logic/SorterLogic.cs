using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
	public class SorterLogic
	{
		/// <summary>
		/// ���������㷨�Ƚ��ٶ��ɿ쵽��
		/// 1.ϣ������	ShellSort
		/// 2.��������	InsertionSort
		/// 3.ѡ������	SelectionSort
		/// 4.ð������	BubbleSort
		/// </summary>
		public void Comparison()
		{
			Random random = new Random();
			int intCount = 99;
			int[] array = new int[intCount];
			for ( int i = 0 ; i < intCount ; i++ )
			{
				array[i] = random.Next(0 , intCount);
			}

			//this.ShellSort(array , SortOrder.Ascending);
			//foreach ( int intPrint in array )
			//{
			//    Console.Write(intPrint + ",");
			//}
			//Console.WriteLine();
			//this.ShellSort(array , SortOrder.Descending);
			//foreach ( int intPrint in array )
			//{
			//    Console.Write(intPrint + ",");
			//}
			//Console.WriteLine();

			DateTime dateTimeShellSort = DateTime.Now;
			this.ShellSort(array , SortOrder.Ascending);
			this.ShellSort(array , SortOrder.Descending);
			TimeSpan timeSpanShellSort = DateTime.Now - dateTimeShellSort;

			DateTime dateTimeInsertionSort = DateTime.Now;
			this.InsertionSort(array , SortOrder.Ascending);
			this.InsertionSort(array , SortOrder.Descending);
			TimeSpan timeSpanInsertionSort = DateTime.Now - dateTimeInsertionSort;

			DateTime dateTimeSelectionSort = DateTime.Now;
			this.SelectionSort(array , SortOrder.Ascending);
			this.SelectionSort(array , SortOrder.Descending);
			TimeSpan timeSpanSelectionSort = DateTime.Now - dateTimeSelectionSort;

			DateTime dateTimeBubbleSort = DateTime.Now;
			this.BubbleSort(array , SortOrder.Ascending);
			this.BubbleSort(array , SortOrder.Descending);
			TimeSpan timeSpanBubbleSort = DateTime.Now - dateTimeBubbleSort;

			Console.WriteLine("ShellSort:     {0} " , timeSpanShellSort.TotalSeconds);
			Console.WriteLine("InsertionSort: {0} " , timeSpanInsertionSort.TotalSeconds);
			Console.WriteLine("SelectionSort: {0} " , timeSpanSelectionSort.TotalSeconds);
			Console.WriteLine("BubbleSort:    {0} " , timeSpanBubbleSort.TotalSeconds);
			Console.ReadKey();
		}

		/// <summary>
		/// ϣ������(Shell)
		/// </summary>
		/// <param name="list"></param>
		/// <param name="SortOrder"></param>
		public void ShellSort(int[] list , SortOrder SortOrder)
		{
			int inc;
			switch ( SortOrder )
			{
				case SortOrder.Ascending://����
					for ( inc = 1 ; inc <= list.Length / 9 ; inc = 3 * inc + 1 )
					{
						for ( ; inc > 0 ; inc /= 3 )
						{
							for ( int i = inc + 1 ; i <= list.Length ; i += inc )
							{
								int t = list[i - 1];
								int j = i;
								while ( ( j > inc ) && ( list[j - inc - 1] > t ) )
								{
									list[j - 1] = list[j - inc - 1];
									j -= inc;
								}
								list[j - 1] = t;
							}
						}
					}
					break;
				case SortOrder.Descending://����
					for ( inc = 1 ; inc <= list.Length / 9 ; inc = 3 * inc + 1 )
					{
						for ( ; inc > 0 ; inc /= 3 )
						{
							for ( int i = inc + 1 ; i <= list.Length ; i += inc )
							{
								int t = list[i - 1];
								int j = i;
								while ( ( j > inc ) && ( list[j - inc - 1] < t ) )
								{
									list[j - 1] = list[j - inc - 1];
									j -= inc;
								}
								list[j - 1] = t;
							}
						}
					}
					break;
				case SortOrder.None://������
					break;
			}
		}//end void sort

		/// <summary>
		/// ��������(Insertion)
		/// </summary>
		/// <param name="list"></param>
		/// <param name="SortOrder"></param>
		public void InsertionSort(int[] list , SortOrder SortOrder)
		{
			switch ( SortOrder )
			{
				case SortOrder.Ascending://����
					for ( int i = 1 ; i < list.Length ; i++ )
					{
						int t = list[i];
						int j = i;
						while ( ( j > 0 ) && ( list[j - 1] > t ) )
						{
							list[j] = list[j - 1];
							--j;
						}
						list[j] = t;
					}//end for
					break;
				case SortOrder.Descending://����
					for ( int i = 1 ; i < list.Length ; i++ )
					{
						int t = list[i];
						int j = i;
						while ( ( j > 0 ) && ( list[j - 1] < t ) )
						{
							list[j] = list[j - 1];
							--j;
						}
						list[j] = t;
					}//end for
					break;
				case SortOrder.None://������
					break;
			}
		}//end void sort

		private int min;
		/// <summary>
		/// ѡ������(Selection)
		/// </summary>
		/// <param name="list"></param>
		/// <param name="SortOrder"></param>
		public void SelectionSort(int[] list , SortOrder SortOrder)
		{
			switch ( SortOrder )
			{
				case SortOrder.Ascending://����
					for ( int i = 0 ; i < list.Length - 1 ; i++ )
					{
						min = i;
						for ( int j = i + 1 ; j < list.Length ; j++ )
						{
							if ( list[j] < list[min] )
								min = j;
						}

						int t = list[min];
						list[min] = list[i];
						list[i] = t;
					}//endfor
					break;
				case SortOrder.Descending://����
					for ( int i = 0 ; i < list.Length - 1 ; i++ )
					{
						min = i;
						for ( int j = i + 1 ; j < list.Length ; j++ )
						{
							if ( list[j] > list[min] )
								min = j;
						}

						int t = list[min];
						list[min] = list[i];
						list[i] = t;
					}//endfor
					break;
				case SortOrder.None://������
					break;
			}
		}//end void sort

		/// <summary>
		/// ð������(Bubble)
		/// </summary>
		/// <param name="list"></param>
		/// <param name="SortOrder"></param>
		public void BubbleSort(int[] list , SortOrder SortOrder)
		{
			int i , j , temp;
			bool done = false;
			j = 1;
			switch ( SortOrder )
			{
				case SortOrder.Ascending://����
					while ( ( j < list.Length ) && ( !done ) )
					{
						done = true;
						for ( i = 0 ; i < list.Length - j ; i++ )
						{
							if ( list[i] > list[i + 1] )
							{
								done = false;
								temp = list[i];
								list[i] = list[i + 1];
								list[i + 1] = temp;
							}//end if
						}//end for
						j++;
					}//end while
					break;
				case SortOrder.Descending://����
					while ( ( j < list.Length ) && ( !done ) )
					{
						done = true;
						for ( i = 0 ; i < list.Length - j ; i++ )
						{
							if ( list[i] < list[i + 1] )
							{
								done = false;
								temp = list[i];
								list[i] = list[i + 1];
								list[i + 1] = temp;
							}//end if
						}//end for
						j++;
					}//end while
					break;
				case SortOrder.None://������
					break;
			}

		}//end viod sort
	}

	/// <summary>
	/// ��ҳ������ʽMethod
	/// </summary>
	public enum SortOrder
	{
		None,
		Ascending,
		Descending
	}
}
