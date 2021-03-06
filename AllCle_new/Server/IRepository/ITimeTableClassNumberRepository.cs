﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models;

namespace Server.IRepository
{
    public interface ITimeTableClassNumberRepository
    {
        List<string> GetTimeTableClassNumbers(string _id, string TimeTableName);
        void PostTimeTable(TimeTableClassNumber _timeTableClassNumber);
        bool GetClassNumber(string ID, string TimeTableName, string ClassNumber);
        List<TimeTableClassNumber> GetClassNumber2(string ID, string TimeTableName);

        //void UpdateTimeTableClassNumber(UpdateUserIdTimeTable _timeTableClassNumber);
    }
}
