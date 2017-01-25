using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastLookup
{
    class LookupArray
    {
        struct LookupItem
        {
            public int ID { get; set; }
            public int Weight { get; set; }
        }

        int _keyWidth = 0;
        int _lookupHeight = 0;
        char[] _alphabet = null;
        Dictionary<char, int> _alphakey = null;
        LookupItem[][] _array = null;

        public LookupArray(int keyWidth, int lookupHeight, char[] alphabet)
        {
            _keyWidth = keyWidth;
            _lookupHeight = lookupHeight;
            _alphabet = alphabet;
            _alphakey = new Dictionary<char, int>();
            int ix = 0;
            foreach (char c in alphabet)
                _alphakey[c] = ix++;
            int len = 1;
            for (int i = 0; i < _keyWidth; i++)
                len *= _alphabet.Length;
            _array = new LookupItem[len][];
        }

        public void AddLookupName(int id, string name)
        {
            int iw = 0;
            foreach (string word in ExtractWord(name))
            {
                for (int i = 0; i < word.Length - _keyWidth + 1; i++)
                {
                    int ix = IndexKey(word.Substring(i, _keyWidth));
                    int wt = i * 10 + iw;

                    if (_array[ix] == null)
                    {
                        _array[ix] = new LookupItem[_lookupHeight];
                        for (int j = 0; j < _lookupHeight; j++)
                        {
                            _array[ix][j].ID = -1;
                            _array[ix][j].Weight = int.MaxValue;
                        }
                    }

                    int jx = _lookupHeight - 1;
                    for (int j = 0; j < _lookupHeight; j++)
                    {
                        if (_array[ix][j].ID == id)
                        {
                            if (_array[ix][j].Weight > wt)
                            {
                                _array[ix][j].Weight = wt;
                                jx = j - 1;
                            }
                            break;
                        }
                    }

                    while (jx >= 0)
                    {
                        if (jx == _lookupHeight - 1)
                        {
                            if (_array[ix][jx].Weight <= wt) break;
                            _array[ix][jx].ID = id;
                            _array[ix][jx].Weight = wt;
                        }
                        if (jx < _lookupHeight - 1)
                        {
                            if (_array[ix][jx].Weight <= _array[ix][jx + 1].Weight) break;
                            int tID = _array[ix][jx].ID;
                            int tWT = _array[ix][jx].Weight;
                            _array[ix][jx].ID = _array[ix][jx + 1].ID;
                            _array[ix][jx].Weight = _array[ix][jx + 1].Weight;
                            _array[ix][jx + 1].ID = tID;
                            _array[ix][jx + 1].Weight = tWT;
                        }
                        jx--;
                    }
                }
                iw++;
            }
        }

        public IEnumerable<string> Export()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("lookupName");
            for (int i = 0; i < _lookupHeight; i++)
                sb.AppendFormat(",ID_{0}", i);
            yield return sb.ToString();
            sb.Clear();
            for (int i = 0; i < _array.Length; i++)
                if (_array[i] != null)
                {
                    sb.Append(DeindexKey(i));
                    for(int j = 0; j < _lookupHeight; j++)
                        sb.AppendFormat(",{0}", _array[i][j].ID > -1 ? _array[i][j].ID.ToString() : "");
                    yield return sb.ToString();
                    sb.Clear();
                }
        }

        int IndexKey(string key)
        {
            int index = 0;
            for (int i = 0; i < _keyWidth; i++)
                index = index * _alphabet.Length + _alphakey[key[i]];
            return index;
        }
        string DeindexKey(int index)
        {
            char[] key = new char[_keyWidth];
            for (int i = 0; i < _keyWidth; i++)
            {
                key[_keyWidth - 1 - i] = _alphabet[index % _alphabet.Length];
                index /= _alphabet.Length;
            }
            return new string(key);
        }
        IEnumerable<string> ExtractWord(string s)
        {
            int ix = 0;
            int len = 0;
            while (ix < s.Length)
            {
                while (ix < s.Length && !_alphakey.ContainsKey(s[ix])) ix++;
                if (ix == s.Length) break;
                while (ix + len < s.Length && _alphakey.ContainsKey(s[ix + len])) len++;
                yield return s.Substring(ix, len);
                ix += len;
                len = 0;
            }
        }
    }
}
