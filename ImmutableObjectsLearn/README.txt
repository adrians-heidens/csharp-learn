Example of creating immutable objects.

* all information injected into the class should be supplied in the constructor
* all properties should be getters only
* if a collection (or Array) is passed into the constructor, it should be copied to keep the caller from modifying it later
* if you're going to return your collection, either return a copy or a read-only version (for example, using ArrayList.ReadOnly or similar
* keep in mind that you still may have the appearance of a mutable class if any of your members are mutable

From: https://stackoverflow.com/a/352493