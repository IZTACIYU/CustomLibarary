using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;


namespace Nexu.Utility
{
    public interface IUndoable
    {
        public struct Command
        {
            public Action Execute { get; set; }
            public Action Undo { get; set; }

            public Command(Action _Execute, Action _Undo)
            {
                this.Execute = _Execute;
                this.Undo = _Undo;
            }
        }
        public Stack<Command> CommandStack { get; set; }
    }

    static public class Util
    {
        /***********************************************************************
        *            Newtonsoft.Json을 기반으로 작성된 클래스임을 알림
        *     프로젝트 - NuGet 패티지 관리 - Newtonsoft.Json 다운로드 후 사용
        ***********************************************************************/


        /***********************************************************************
        *                             Json Methods
        ***********************************************************************/
        #region .
        /// <summary>
        /// 지정 데이터를 Json 파일로 저장합니다.
        /// <para>대응범위 : 직렬/역직렬화가 가능한 모든 범위</para>
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="dataType">저장하고자 하는 데이터</param>
        /// <param name="filePath">저장 경로</param>
        /// <param name="fileName">파일명</param>
        /// <returns>Void Value</returns>
        static public void SaveJSON<T>(this T dataType, string filePath, string fileName)
        {
            if (string.IsNullOrEmpty(filePath))
            {
#if UNITY
                UnityEngine.Debug.LogError("Error: File path is null or empty.");
#else
                Console.WriteLine("Error: File path is null or empty.");
#endif
                return; // 파일 경로가 유효하지 않으므로 메서드 종료
            }

            string CPath = Path.Combine(filePath, fileName) + ".json";

            try
            {
                string json = JsonConvert.SerializeObject(dataType, Formatting.Indented);
                File.WriteAllText(CPath, json);

                "SUC".print();
            }
            catch (Exception ex)
            {
#if UNITY
                UnityEngine.Debug.LogError($"Error: Failed to save data to JSON. {ex.Message}");
#else
                Console.WriteLine($"Error: Failed to save data to JSON. {ex.Message}");
#endif
            }
        }


        /// <summary>
        /// Json 파일에서 데이터를 복사합니다.
        /// <para>대입 가능 범위 내 데이터 복사를 지원합니다.</para>
        /// <para>대응범위 : 직렬/역직렬화가 가능한 모든 범위</para>
        /// </summary>
        /// <param name="filePath">원본 파일 경로</param>
        /// <param name="copyPath">복사할 파일 경로</param>
        /// <param name="copyPath">복사할 파일명</param>
        /// <returns></returns>
        static public void CopyJSON(string filePath, string copyPath, string copyName)
        {
            if (string.IsNullOrEmpty(filePath))
            {
#if UNITY
                UnityEngine.Debug.LogError("Error: File path is null or empty.");
#else
                Console.WriteLine("Error: File path is null or empty.");
#endif
            }
            if (string.IsNullOrEmpty(copyPath))
            {
#if UNITY
                UnityEngine.Debug.LogError("Error: Copy path is null or empty.");
#else
                Console.WriteLine("Error: Copy path is null or empty.");
#endif
            }
            if (File.Exists(filePath) && File.Exists(copyPath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var format = JsonConvert.DeserializeObject(json);
                    Util.SaveJSON(format, copyPath, copyName);
                }
                catch (Exception ex)
                {
#if UNITY
                    UnityEngine.Debug.LogError($"Error: Failed to load data from JSON. {ex.Message}");
#else
                    Console.WriteLine($"Error: Failed to load data from JSON. {ex.Message}");
#endif
                }
            }
            else
            {
#if UNITY
                UnityEngine.Debug.LogError("Error: File does not exist.");
#else
                Console.WriteLine("Error: File does not exist.");
#endif
            }
        }


        /// <summary>
        /// 지정 Json파일의 모든 데이터를 삭제합니다.
        /// </summary>
        /// <param name="filePath">지정 파일 경로</param>
        /// <param name="fileName">지정 파일명</param>
        static public void RemoveJSON(string filePath, string fileName)
        {
            string json = "";
            SaveJSON(json, filePath, fileName);
        }


        /// <summary>
        /// 지정 Json파일을 삭제합니다.
        /// </summary>
        /// <param name="filePath">지정 파일 경로</param>
        /// <param name="fileName">지정 파일명</param>
        static public void DeleteJSON(string filePath, string fileName) => File.Delete(filePath);


        /// <summary>
        /// Json 파일에서 데이터를 로드합니다.
        /// <para>대입 가능 범위 내 데이터 형식변환을 지원합니다.</para>
        /// <para>대응범위 : 직렬/역직렬화가 가능한 모든 범위</para>
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="dataType">로드하고자 하는 데이터 타입 지정</param>
        /// <param name="filePath">파일 경로</param>
        /// <param name="fileName">파일명</param>
        /// <returns>데이터 객체</returns>
        static public T LoadJSON<T>(string filePath, string fileName)
        {
            string CPath = Path.Combine(filePath, fileName) + ".json";

            if (string.IsNullOrEmpty(CPath))
            {
#if UNITY
                UnityEngine.Debug.LogError("Error: File path is null or empty.");
#else
                Console.WriteLine("Error: File path is null or empty.");
#endif
                return default(T); // 파일 경로가 유효하지 않으므로 기본값 반환
            }

            if (File.Exists(CPath))
            {
                try
                {
                    string json = File.ReadAllText(CPath);
                    var format = JsonConvert.DeserializeObject<T>(json);
                    return format;
                }
                catch (Exception ex)
                {
#if UNITY
                    UnityEngine.Debug.LogError($"Error: Failed to load data from JSON. {ex.Message}");
#else
                    Console.WriteLine($"Error: Failed to load data from JSON. {ex.Message}");
#endif
                    return default(T);
                }
            }
            else
            {
#if UNITY
                UnityEngine.Debug.LogError("Error: File does not exist.");
#else
                Console.WriteLine("Error: File does not exist.");
#endif
                return default(T);
            }
        }


        /// <summary>
        /// 두 Json을 병합시킵니다.
        /// <para>대입 가능 범위 내 데이터 형식변환을 지원합니다.</para>
        /// <para>대응범위 : 직렬/역직렬화가 가능한 모든 범위</para>
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="source">합치고자 하는 데이터</param>
        /// <param name="target">합치고자 하는 데이터</param>
        /// <returns>문자열 형식으로 반환됩니다.</returns>
        static public string MergeJSON<T>(params T[] source)
        {
            string json = null;

            for (int i = 0; i < source.Length; i++)
            {
                if (i == 0)
                {
                    var at = JsonConvert.SerializeObject(source[i], Formatting.Indented).ToCharArray();
                    var pt = at[..(at.Length - 1)];
                    json += new string(pt);
                }
                else if (i > 0 && i < source.Length - 1)
                {
                    var at = JsonConvert.SerializeObject(source[i], Formatting.Indented).ToCharArray();
                    var pt = at[1..(at.Length - 1)];
                    json += new string(pt);
                }
                else
                {
                    var at = JsonConvert.SerializeObject(source[i], Formatting.Indented).ToCharArray();
                    var pt = at[1..];
                    json += new string(pt);
                }
            }

            return json;
        }
        #endregion


        /***********************************************************************
        *                             Unity Method
        ***********************************************************************/
        #region .
        /// <summary>
        /// 이름을 통한 인덱스 파싱
        /// <para>획일화된 이름 유형이 요구됨</para>
        /// </summary>
        /// <param name="name">인덱스를 구하고자 하는 객체의 이름</param>
        /// <param name="mark">객체 이름을 나눌 기호</param>
        /// <param name="index">몇번째 절을 받아올지 지정</param>
        /// <returns></returns>
        static public int IndexParsing(string name, char mark, int index)
        {
            var newType = name.Split(mark);
            return int.Parse(newType[index]);
        }
        #endregion


        /***********************************************************************
        *                        Condition Check Methods
        ***********************************************************************/
        #region .
        /// <summary>
        /// 지정된 데이터 값의 유무를 확인합니다.
        /// <para>대응범위 : IEnumerable, IDictionary 기반 자료구조 범위</para>
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <typeparam name="S">Generic Type</typeparam>
        /// <param name="dataType">찾고자 하는 데이터 값이 들어있는 데이터 형식</param>
        /// <param name="data">찾고자 하는 데이터 값</param>
        /// <returns>True / False / Null</returns>
        static public bool? Contains<T, S>(T dataType, S data) where T : IEnumerable
        {
            if (dataType is IDictionary Dictionary)
            {   // data가 있으면 True, 없으면 False 반환
                return Dictionary.Values.Cast<object>().Any(value => value != null && value.Equals(data));
            }
            else if (dataType is IEnumerable numerable)
            {
                return numerable.Cast<object>().Any(value => value != null && value.Equals(data));
            }
            else
                return null;
        }
        #endregion


        /***********************************************************************
        *                      Data Struct Modify Methods
        ***********************************************************************/
        #region .
        /// <summary>
        /// 지정된 데이터 형식에서 키를 기반으로 페어를 삭제합니다.
        /// <para>대응범위 : IDictionary 기반 자료구조 범위</para>
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <typeparam name="S">Generic Type</typeparam>
        /// <param name="dataType">작업을 수행하고자 하는 데이터 형식</param>
        /// <param name="data">삭제하고자 하는 키값</param>
        /// <returns></returns>
        static public T RemoveDataKeyPair<T, S>(T dataType, S data) where T : IDictionary
        {
            if (dataType != null)
            {
                if (dataType.Contains(data))
                {
                    dataType.Remove(data);
                    return dataType;
                }
                else
                    return default(T);
            }
            else
            {
#if UNITY
                UnityEngine.Debug.LogError("Error: Null Value Detected.");
#else
                Console.WriteLine("Error: Null Value Detected.");
#endif
                return default(T);
            }
        }
        /// <summary>
        /// 지정된 데이터 형식에서 특정 값을 삭제합니다.
        /// <para>대응 범위 : class, IEnumerable, new()</para>
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <typeparam name="S">Generic Type</typeparam>
        /// <param name="dataType">작업을 수행하고자 하는 데이터 형식</param>
        /// <param name="data">삭제하고자 하는 값</param>
        /// <returns></returns>
        static public T RemoveDataValue<T, S>(T dataType, S data) where T : class, IEnumerable<S>, new()
        {
            if (dataType is ICollection<S> collection)
            {
                collection.Remove(data);
                return dataType;
            }
            else if (dataType is Stack<S> stack)
            {
                var procStack = new Stack<S>();
                foreach (var item in stack)
                {
                    if (!item.Equals(data))
                    {
                        procStack.Push(item);
                    }
                }
                var newType = new Stack<S>();
                foreach (var item in procStack)
                {
                    newType.Push(item);
                }
                return (T)(object)newType;
            }
            else if (dataType is Queue<S> queue)
            {
                var newType = new Queue<S>();
                foreach (var item in queue)
                {
                    if (!item.Equals(data))
                    {
                        newType.Enqueue(item);
                    }
                }
                return (T)(object)newType;
            }


            return default(T);
        }
        /// <summary>
        /// 지정된 배열 형식에서 특정 값을 삭제합니다.
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="dataType">작업을 수행하고자 하는 배열 형식</param>
        /// <param name="data">삭제하고자 하는 값</param>
        /// <returns></returns>
        static public T[] RemoveArrayValue<T>(T[] dataType, T data)
        {
            var newType = new List<T>();

            foreach (var item in dataType)
            {
                if (!item.Equals(data))
                {
                    newType.Add(item);
                }
            }
            return newType.ToArray();
        }
        #endregion


        /***********************************************************************
        *                          Extension Methods
        ***********************************************************************/
        #region .
        /// <summary>
        /// 출력 확장 메서드
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="value">Data</param>
        static public void print<T>(this T value)
        {
            Console.WriteLine(value);
        }



        /// <summary>
        /// 아스키 변환 확장 메서드
        /// </summary>
        /// <param name="str">목표 문자열</param>
        /// <returns>정수형 배열</returns>
        static public int[] toAscii(this string str)
        {
            var chr = str.ToCharArray();
            var arry = new int[chr.Length];
            for (int i = 0; i < chr.Length; i++)
            {
                arry[i] = (int)chr[i];
            }

            return arry;
        }



        /// <summary>
        /// 정수 변환 확장 메서드
        /// </summary>
        /// <param name="str">목표 문자열</param>
        /// <returns>정수형</returns>
        static public int toInt(this string str)
        {
            return int.Parse(str);
        }

        /// <summary>
        /// 정수 변환 확장 메서드
        /// </summary>
        /// <param name="str">목표 문자</param>
        /// <returns>정수형</returns>
        static public int toInt(this char chr)
        {
            return int.Parse(chr.ToString());
        }



        #region 더하기
        /// <summary>
        /// 더하기 확장 매서드
        /// </summary>
        /// <param name="tuple">두개의 int형</param>
        /// <returns>int형</returns>
        static public int add(this (int, int) tuple)
        {
            return tuple.Item1 + tuple.Item2;
        }
        /// <summary>
        /// 더하기 확장 매서드
        /// </summary>
        /// <param name="tuple">두개의 float형</param>
        /// <returns>float형</returns>
        static public float add(this (float, float) tuple)
        {
            return tuple.Item1 + tuple.Item2;
        }
        /// <summary>
        /// 더하기 확장 매서드
        /// </summary>
        /// <param name="tuple">두개의 double형</param>
        /// <returns>double형</returns>
        static public double add(this (double, double) tuple)
        {
            return tuple.Item1 + tuple.Item2;
        }
        /// <summary>
        /// 더하기 확장 매서드
        /// </summary>
        /// <param name="tuple">두개의 string형</param>
        /// <returns>string형</returns>
        static public string add(this (string, string) tuple)
        {
            return tuple.Item1 + tuple.Item2;
        }


        /// <summary>
        /// 더하기 확장 매서드
        /// </summary>
        /// <param name="data">베이스 int형</param>
        /// <param name="value">더하고자 하는 int형</param>
        /// <returns>int형</returns>
        static public int add(this int data, int value)
        {
            return data + value;
        }
        /// <summary>
        /// 더하기 확장 매서드
        /// </summary>
        /// <param name="data">베이스 float형</param>
        /// <param name="value">더하고자 하는 float형</param>
        /// <returns>float형</returns>
        static public float add(this float data, float value)
        {
            return data + value;
        }
        /// <summary>
        /// 더하기 확장 매서드
        /// </summary>
        /// <param name="data">베이스 double형</param>
        /// <param name="value">더하고자 하는 double형</param>
        /// <returns>double형</returns>
        static public double add(this double data, double value)
        {
            return data + value;
        }
        /// <summary>
        /// 더하기 확장 매서드
        /// </summary>
        /// <param name="data">베이스 string형</param>
        /// <param name="value">더하고자 하는 string형</param>
        /// <returns>string형</returns>
        static public string add(this string data, string value)
        {
            return data + value;
        }
        #endregion

        #region 곱하기
        /// <summary>
        /// 곱하기 확장 매서드
        /// </summary>
        /// <param name="tuple">두개의 int형</param>
        /// <returns>int형</returns>
        static public int multiple(this (int, int) tuple)
        {
            return tuple.Item1 * tuple.Item2;
        }



        /// <summary>
        /// 곱하기 확장 매서드
        /// </summary>
        /// <param name="tuple">두개의 float형</param>
        /// <returns>float형</returns>
        static public float multiple(this (float, float) tuple)
        {
            return tuple.Item1 * tuple.Item2;
        }



        /// <summary>
        /// 곱하기 확장 매서드
        /// </summary>
        /// <param name="tuple">두개의 double형</param>
        /// <returns>double형</returns>
        static public double multiple(this (double, double) tuple)
        {
            return tuple.Item1 * tuple.Item2;
        }
        #endregion

        #region 범위
        static public int max(this int value, int max) => value > max ? max : value;
        static public int min(this int value, int min) => value < min ? min : value;

        static public float max(this float value, float max) => value > max ? max : value;
        static public float min(this float value, float min) => value < min ? min : value;

        static public int range(this int value, int min, int max) => value.min(min).max(max);
        static public float range(this float value, float min, float max) => value.min(min).max(max);
        #endregion
        
        #endregion
    }
}