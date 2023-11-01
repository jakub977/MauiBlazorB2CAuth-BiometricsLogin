﻿using System.Runtime.Serialization;

namespace Principal.Telemedicine.Shared.Api
{
    /// <summary>
    /// Třída pro návratové hodnoty API
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class GenericResponse<TResult> : IGenericResponse<TResult>
    {
        private TResult? _data;
        private bool _success = false;
        private string? _message;
        private string? _detail;
        private int _code = -1;
        private int _records = 0;

        [DataMember()]
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        [DataMember()]
        public string? Message
        {
            get { return _message; }
            set { _message = value; }
        }

        [DataMember()]
        public string? Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

        [DataMember()]
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        [DataMember()]
        public int Records
        {
            get { return _records; }
            set { _records = value; }
        }

        [DataMember()]
        public TResult? Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public GenericResponse(TResult? data, bool? success = null, int ?code = null, string? message = null, string? detail = null, int? records = null)
        {
            if (data != null)
                _data = data;
            if (success != null)
                _success = success.Value;
            if (code != null)
                _code = code.Value;
            if (message != null)
                _message = message;
            if (detail != null)
                _detail = detail;
            if (records != null)
                _records = records.Value;
        }
    }
}
