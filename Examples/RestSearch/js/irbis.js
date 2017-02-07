var serviceUrl = "http://localhost:1234/";
var irbisBusy = false;

function addOption(list, value, text)
{
    var el = document.createElement("option");
    el.value = value;
    el.textContent = text;
    list.appendChild(el);
}

function clearList(list)
{
    for (i = list.options.length - 1; i >= 0; i--)
    {
        list.remove(i);
    }
}

