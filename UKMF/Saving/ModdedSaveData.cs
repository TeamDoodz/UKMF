using System;
using System.Collections.Generic;
using System.IO;

namespace UKMF.Saving {
	public class ModdedSaveData {
		private Dictionary<string, int> ints = new Dictionary<string, int>();
		private Dictionary<string, float> floats = new Dictionary<string, float>();
		private Dictionary<string, string> strings = new Dictionary<string, string>();
		private Dictionary<string, bool> bools = new Dictionary<string, bool>();
		private Dictionary<string, byte[]> binary = new Dictionary<string, byte[]>();

		/// <summary>
		/// Whether or not any changes have been made to this instance since the last time it was saved.
		/// </summary>
		public bool Dirty { get; set; }

		public int GetInt(string key, int fallback = 0) {
			if(!ints.ContainsKey(key)) return fallback;
			return ints[key];
		}
		public void SetInt(string key, int value) {
			if(ints.ContainsKey(key)) {
				ints[key] = value;
			} else {
				ints.Add(key, value);
			}
			Dirty = true;
		}

		public float GetFloat(string key, float fallback = 0f) {
			if(!floats.ContainsKey(key)) return fallback;
			return floats[key];
		}
		public void SetFloat(string key, float value) {
			if(floats.ContainsKey(key)) {
				floats[key] = value;
			} else {
				floats.Add(key, value);
			}
			Dirty = true;
		}

		public string GetString(string key, string fallback = "") {
			if(!strings.ContainsKey(key)) return fallback;
			return strings[key];
		}
		public void SetString(string key, string value) {
			if(strings.ContainsKey(key)) {
				strings[key] = value;
			} else {
				strings.Add(key, value);
			}
			Dirty = true;
		}

		public bool GetBool(string key, bool fallback = false) {
			if(!bools.ContainsKey(key)) return fallback;
			return bools[key];
		}
		public void SetBool(string key, bool value) {
			if(bools.ContainsKey(key)) {
				bools[key] = value;
			} else {
				bools.Add(key, value);
			}
			Dirty = true;
		}

		public bool BytesExist(string key) => binary.ContainsKey(key);
		public byte[] GetBytes(string key) {
			if(!BytesExist(key)) throw new KeyNotFoundException($"Byte array \"{key}\" was not found.");
			return binary[key];
		}
		public void SetBytes(string key, byte[] value) {
			if(binary.ContainsKey(key)) {
				binary[key] = value;
			} else {
				binary.Add(key, value);
			}
			Dirty = true;
		}

		/// <summary>
		/// Writes to the specified <see cref="BinaryWriter"/> with data about this object.
		/// </summary>
		public void Serialize(BinaryWriter writer) {
			void WriteAll<T>(Dictionary<string,T> data, Action<T> WriteData) {
				writer.Write(data.Count);
				foreach(string key in data.Keys) {
					writer.Write(key);
				}
				foreach(T value in data.Values) {
					WriteData(value);
				}
			}

			WriteAll(ints, (x) => writer.Write(x));
			WriteAll(floats, (x) => writer.Write(x));
			WriteAll(strings, (x) => writer.Write(x));
			WriteAll(bools, (x) => writer.Write(x));
			WriteAll(binary, (x) => {
				//TODO: Dynamically-lengthed binary
				writer.Write(x.Length);
				writer.Write(x);
			});
		}

		/// <summary>
		/// Reads data from the specified <see cref="BinaryReader"/> and applies it to this object.
		/// </summary>
		public void Deserialize(BinaryReader reader) {
			void ReadAll<T>(Dictionary<string,T> data, Func<T> ReadData) {
				data.Clear();

				int length = reader.ReadInt32();
				string[] keys = new string[length];
				T[] values = new T[length];

				for(int i = 0; i < length; i++) {
					keys[i] = reader.ReadString();
				}
				for(int i = 0; i < length; i++) {
					values[i] = ReadData();
				}

				for(int i = 0; i < length; i++) {
					data.Add(keys[i], values[i]);
				}
			}

			ReadAll(ints, () => reader.ReadInt32());
			ReadAll(floats, () => reader.ReadSingle());
			ReadAll(strings, () => reader.ReadString());
			ReadAll(bools, () => reader.ReadBoolean());
			ReadAll(binary, () => {
				int length = reader.ReadInt32();
				return reader.ReadBytes(length);
			});
		}
	}
}
