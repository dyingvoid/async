Func<CoffeeBeans, CoffeeGround> grindCoffee = coffeeBeans => new CoffeeGround(coffeeBeans);
Func<CoffeeGround, Espresso> brewCoffee = coffeeGround => new Espresso(coffeeGround);
Func<CoffeeBeans, Espresso> cookCoffee1 = coffeeBeans => brewCoffee(grindCoffee(coffeeBeans));
Func<CoffeeBeans, Espresso> cookCoffee2 = grindCoffee.Compose(brewCoffee);

var espresso = cookCoffee2(new CoffeeBeans());
Console.WriteLine(espresso);

class CoffeeBeans { }

class CoffeeGround(CoffeeBeans coffeeBeans)
{
    public CoffeeBeans Beans { get; set; } = coffeeBeans;
}

abstract class Coffee { }

class Espresso(CoffeeGround ground) : Coffee {
    public override string ToString()
    {
        return "espresso";
    }
}
