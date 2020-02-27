namespace Lab2
{
    public interface ICPUQueue
    {
        CPUProcess Get();
        void Put(CPUProcess process);
    }
}