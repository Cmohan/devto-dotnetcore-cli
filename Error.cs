class Error
{
    public string error { get; set; }
    public int status { get; set; }

    public Error (){
        this.error = "";
        this.status = 0;
    }
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
            return $"\nError: {status} - {error}\n";
        }
        else
        {
            return $"\nError: {error}\n";
        }
    }
}