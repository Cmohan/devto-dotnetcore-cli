class Error
{
    public string error { get; set; }
    public int status { get; set; }

    public Error (string error)
    {
        this.error = error;
        this.status = 0;
    }
    public Error (string error, int status)
    {
        this.error = error;
        this.status = status;
    }

    public override string ToString()
    {
        if (status != 0)
        {
            return $"Error: {status} - {error}";
        }
        else
        {
            return $"Error: {error}";
        }
    }
}