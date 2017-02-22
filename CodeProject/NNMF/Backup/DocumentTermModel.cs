using System;
using System.Collections.Generic;
using System.Text;

namespace NNMFSearchResultClustering
{
	class DocumentTermModel
	{
		Dictionary<string, int> _termToIndex = new Dictionary<string, int>();
		Dictionary<int, string> _indexToTerm = new Dictionary<int, string>();
		public class Document
		{
			public Dictionary<int, uint> termCount = new Dictionary<int, uint>();
			public int index;
			public uint totalTermCount = 0;
			public object obj;
			public Document(int index, object obj) { this.index = index; this.obj = obj; }
		}
		List<Document> _documentList = new List<Document>();
		Dictionary<int, Dictionary<Document, bool>> _keywordDocumentOccurence = new Dictionary<int, Dictionary<Document, bool>>();

		public DocumentTermModel()
		{
		}

		public Document AddDocument(object obj)
		{
			Document ret = new Document(_documentList.Count, obj);
			_documentList.Add(ret);
			return ret;
		}
		public void AddTerm(Document doc, string term, uint count)
		{
			int index;
			if(!_termToIndex.TryGetValue(term, out index)) {
				index = _termToIndex.Count;
				_termToIndex.Add(term, index);
				_indexToTerm.Add(index, term);
			}
			doc.termCount.Add(index, count);
			doc.totalTermCount += count;

			Dictionary<Document, bool> termBag;
			if(!_keywordDocumentOccurence.TryGetValue(index, out termBag))
				_keywordDocumentOccurence[index] = termBag = new Dictionary<Document, bool>();
			termBag[doc] = true;
		}
		public NNMFMatrix GetNormalised()
		{
			int numDocs = _documentList.Count;
			int numTerms = _termToIndex.Count;
			double nd = numDocs;
			NNMFMatrix ret = new NNMFMatrix(numTerms, numDocs);
			int x = 0;
			foreach(Document doc in _documentList) {
				double docTermCount = doc.totalTermCount;
				foreach(KeyValuePair<int, uint> item in doc.termCount) {
					int termIndex = item.Key;
					double count = item.Value;
					double termFrequency = count / docTermCount;
					double inverseDocumentFrequency = Math.Log(nd / _keywordDocumentOccurence[termIndex].Count);
					double weight = inverseDocumentFrequency * termFrequency;
					ret.Set(termIndex, x, (float)weight);
				}
				++x;
			}
			return ret;
		}
		public Document GetDocument(int index)
		{
			return _documentList[index];
		}
		public string GetTerm(int index)
		{
			return _indexToTerm[index];
		}
	}
}
