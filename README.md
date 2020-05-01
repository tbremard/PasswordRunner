# PasswordRunner
Began as a password cracker tool, with work on design, SOLID programming principles
Use of Service Locator for dependency injection principle
Use of Interfaces for Producer/Consummer for Open Closed principle
Use of ActionBlock for parallelism
Evolution to be a generic Producer Consummer generic framework.
==> Use of ModuleDefinition to run your own c# assembly with your own code for:
        + Input generation ( producer / password )
        + Check process ( consummer / validation of password)

You can provide your own assembly with:
Producer, which inherits from IPasswordProducer
Consummer, which inherits from IPasswordValidator

Load this assembly via:
  LoadModules(ModuleDefinition producer, ModuleDefinition validator)

with:
    public class ModuleDefinition
    {
        public string BinaryFile { get; set; }
        public string ClassName { get; set; }
        public string Input { get; set; }//should be Json: this is given to constructor
    }

A python script is supplied to benchmark the best number of processors: Optimizer.py


