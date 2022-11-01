using DomainBase;
using System.Collections;

namespace Warehouse.Domain
{
    public class Evidence : ValueObject, IEnumerable<string>
    {
        readonly string separator = Environment.NewLine;
        string _evidenceString;

        public Evidence(string evidenceString)
        {
            _evidenceString = evidenceString;
        }

        public void Append(Evidence proof)
        { }

        public override string ToString()
        {
            return _evidenceString;
        }

        public IEnumerator<string> GetEnumerator()
        {
            string[] evidenceList = _evidenceString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if (evidenceList.Any())
            {
                foreach (var item in evidenceList)
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (string.IsNullOrEmpty(_evidenceString))
                return null;

            return _evidenceString.Split(separator, StringSplitOptions.RemoveEmptyEntries).GetEnumerator();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _evidenceString;
        }
    }
}
