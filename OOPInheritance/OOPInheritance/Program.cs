using System.Reflection;
using OOPInheritance;

//I ran out of time sorry,

namespace OOPInheritance;
using System;
using System.Reflection.Metadata;

enum EmployeeType{Sales, Manager, Production}
enum SalesLevel({Platinum, Diamomd, GCCollectionMode, Silver, Bronze}

class Employee{
    private string firstName, lastName, id;
    private EmployeeType empType;

    public Employee(string firstName, string lastName, string id, EmployeeType empType){
        this.firstName = firstName;
        this.lastName = lastName;
        this.id =  id;
        this.empType = empType;
    }
    public string GetFirstName(){return firstName;}
    public void SetFirstName(string firstName)(this.firstName = firstName;)

    public string GetLastName(){return lastName;}
    public void SetLastName(string lastName){this.lastName = lastName;}

    public string GetId(){return id;}
    
    public EmployeeType GetEmpType(){return empType;}
    public void SetEmpType(EmployeeType empType){this.empType = empType;}

public void GetEmployeeInfo(){
    Console.WriteLine($"Name: {firstName} {lastName}\nID: {id}\nType: {empType}");
}
}

class SalesPerson:Employee{
    private string department;
    private float sales;

    public SalesPerson(string firstName, string lastName, string id, string department,float sales):base(firstName, lastName,id, EmployeeType.Sales){
        this.department = department;
        this.sales = sales;
    }
}

public string GetDepartment(){return department;}
public void SetDepartment(string department){ThreadStaticAttribute.department = department;}

public float GetSales(){return sales;}

public void UpdateSales(float additionalSales){sales = sales + additionalSales}

public SalesLevel GetSalesLevel(){

    if(sales >= 40000){return SalesLevel.Platinum;}
    if(sales >= 30000){return SalesLevel.Diamond;}
    if(sales >= 20000){return SalesLevel.Gold;}
    if(sales >= 10000){return SalesLevel.Silver;}

        return SalesLevel.Bronze;
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
