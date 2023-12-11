function debounce(func, delay) {
    let timeoutId;

    return function () {
        const context = this;
        const args = arguments;

        clearTimeout(timeoutId);

        timeoutId = setTimeout(function () {
            func.apply(context, args);
        }, delay);
    };
}

// Example usage:
const myFunction = () => {
    current = window.location.href;
    let check = current.split('search?q');
    console.log(check)
    if (check.length == 1) {
        window.location.replace(`${current}search?q=${inputElement.value.trim()}`);
    } else {
        window.location.replace(`${check[0]}search?q=${inputElement.value.trim()}`);
    }
};

const debouncedFunction = debounce(myFunction, 1500);

// Attach the debounced function to an input event (e.g., typing)
const inputElement = document.getElementById("your-inputilement");

inputElement.addEventListener("keyup", debouncedFunction);