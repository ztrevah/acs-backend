namespace SystemBackend.Exceptions
{
    public class CivilianNotFoundException : Exception
    {
        public CivilianNotFoundException(string id)
            : base($"The civilian with ID '{id}' is not found.") { }
        public CivilianNotFoundException(string id, Exception innerException)
            : base($"The civilian with ID '{id}' is not found.", innerException) { }
    }

    public class CivilianDuplicateIdException : Exception
    {
        public CivilianDuplicateIdException(string id)
            : base($"The civilian with ID '{id}' is already existed.") { }
        public CivilianDuplicateIdException(string id, Exception innerException)
            : base($"The civilian with ID '{id}' is already existed.", innerException) { }
    }
}
