using System.Collections;


public class Gold
{
    private int gold = 0;
    
    // returns amount as an int.
    // please avoid using if possable only intended for legacy support
    public int getIntAmount();
    
    // think of a new name
    // returns the representation of gold as a string      
    public string getstringRep();
    
    // sets the amount to int passed in.
    public void setAmount(int toAdd);
    
    // sets ammount equal to gold passed in.
    public void setAmount(Gold toAdd);
    
    // adds the int passed in to the;
    public void addAmmount(int toAdd);
    
    // adds the Gold ammount passed in;
    public void addAmmount(Gold toAdd);
    
    /*
    add overload for + * - and maybe /
    as well as
    */ 




}