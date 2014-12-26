namespace Tasks.DomainModel
{
    public enum TaskStatus 
    { 
        NotStarted = 0,     // Nobody worked on it
        InProgress,         // Anyone is working on it
        WorkDone,           // Work completed but not tested on test environment
        Resolved,           // Tested by the developer on test environment
        InTest,             // In test by a tester
        Accepted            // Accepted by the tester
    }
}