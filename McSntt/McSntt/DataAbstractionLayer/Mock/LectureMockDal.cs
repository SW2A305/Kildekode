using System;
using System.Collections.Generic;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer.Mock
{
    public class LectureMockDal : ILectureDal
    {
        private static Dictionary<long, Lecture> _lectures;

        public LectureMockDal(bool useForTests = false)
        {
            if (useForTests || _lectures == null) { _lectures = new Dictionary<long, Lecture>(); }
        }

        public bool Create(params Lecture[] items)
        {
            foreach (Lecture lecture in items)
            {
                lecture.LectureId = this.GetHighestId() + 1;
                _lectures.Add(lecture.LectureId, lecture);
            }

            return true;
        }

        public bool Update(params Lecture[] items)
        {
            foreach (Lecture lecture in items)
            {
                if (lecture.LectureId > 0 && _lectures.ContainsKey(lecture.LectureId)) {
                    _lectures[lecture.LectureId] = lecture;
                }
            }

            return true;
        }

        public bool Delete(params Lecture[] items)
        {
            foreach (Lecture lecture in items) { if (lecture.LectureId > 0) { _lectures.Remove(lecture.LectureId); } }

            return true;
        }

        public IEnumerable<Lecture> GetAll() { return _lectures.Values; }

        public IEnumerable<Lecture> GetAll(Func<Lecture, bool> predicate) { return _lectures.Values.Where(predicate); }

        public Lecture GetOne(long itemId)
        {
            if (_lectures.ContainsKey(itemId)) { return _lectures[itemId]; }

            return null;
        }

        public void LoadData(Lecture item)
        {
            /* Not applicable */
        }

        public void LoadData(IEnumerable<Lecture> items)
        {
            /* Not applicable */
        }

        private long GetHighestId()
        {
            if (_lectures.Count == 0) { return 0; }

            return _lectures.Max(lecture => lecture.Key);
        }
    }
}
