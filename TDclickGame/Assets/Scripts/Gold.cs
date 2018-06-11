using System.Collections;


public class Gold
{
    private int gold = 0;

    // returns amount as an int.
    // please avoid using if possable only intended for legacy support
    public int getIntAmount()
    {
        return gold;
    }

    // think of a new name
    // returns the representation of gold as a string      
    public string getstringRep()
    {
        return gold.ToString();
    }

    // sets the amount to int passed in.
    public void setAmount(int toAdd)
    {
        gold = toAdd;
    }

    // sets ammount equal to gold passed in.
    public void setAmount(Gold toAdd)
    {
        gold = toAdd.getIntAmount();
    }
    
    // adds the int passed in to the;
    public void addAmount(int toAdd)
    {
        if (toAdd + gold >= 0)
        {
            gold += toAdd;
        }
    }

    // adds the Gold ammount passed in;
    public void addAmount(Gold toAdd)
    {
        if (toAdd.getIntAmount() + gold >= 0)
        {
        gold += toAdd.getIntAmount();
        }
    }
    
    /*
    add overload for + * - and maybe /
    as well as
    */ 




}