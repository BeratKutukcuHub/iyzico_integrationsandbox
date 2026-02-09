
const HeaderType = () => {
    return {
        "Content-Type" : "application/json",
        "X-MyToken" : `MyToken ${localStorage.getItem("token")}` ?? null,
        "X-Idempotency-Key" : localStorage.getItem("idempotant_key") ?? null
    }
} 

const FetchCheck = async (requestType, url, body = null) => {
    try{

        var result = await fetch(url,{
            method : requestType,
            headers : HeaderType(),
            body : requestType.toUpperCase() === "GET" ? undefined : JSON.stringify(body)
        });
        
        let response;
        const contentType = result.headers.get("content-type");
        if(contentType && contentType.includes("application/json")) response = await result.json();
        if(contentType && contentType.includes("text/plain")) response = await result.text();
        if(contentType && contentType.includes("text/html")) response = await result.text();

        if (!result.ok) {
            
            if (result.status === 401) localStorage.removeItem("token");
        }
        return await response;
        
        } catch(error){
        console.log(error);
        throw error;
    }
}

const Products = async () => {
    return (await FetchCheck("GET", "http://localhost:5003/api/Product"));
}
const Login = async () => {
    var result = (await FetchCheck("POST", "http://localhost:5003/api/Identity/login",
        {
        email : "example@example.com", 
        passwordHash : "example123"
        }
    ));
    if(result){
        localStorage.setItem("token","MyToken "+ result.token);
    }
    return result;
}


const Order = async (body) => {
    const response = (await FetchCheck("POST", "http://localhost:5003/api/Order/request/iyzico", body));
    
    return response;
}
let currentBlurState = null;

const BlurOptions = (blurRef, state) => {
  if (currentBlurState === state) return;
  currentBlurState = state;

  let blur_container = document.querySelector(".blur-container");

  if (!blur_container) {
    blur_container = document.createElement("div");
    blur_container.classList.add("blur-container");

    const blur_content = document.createElement("div");
    blur_content.classList.add("blur-content");

    blur_container.appendChild(blur_content);
    document.body.appendChild(blur_container);
  }

  const blur_content = blur_container.querySelector(".blur-content");
  blur_content.innerHTML = "";

  blurRef();

  if (state === "ok" || state === "failure") {
    const blur_button = document.createElement("button");
    blur_button.classList.add("blur-button");

    blur_button.onclick = () => {
      document.body.removeChild(blur_container);
      window.location.reload();
      currentBlurState = null;
    };

    blur_content.appendChild(blur_button);
  }
};

const BlurLoading = () => {

    var blur_loading = document.createElement("div");
    blur_loading.classList.add("blur-loading");

    var blur_text = document.createElement("h2");
    blur_text.textContent = "Ödeme İşlemi Bekleniyor.";

    var blur_loader = document.createElement("i");
    blur_loader.classList.add("blur-loader");
    
    blur_loading.appendChild(blur_text);
    blur_loading.appendChild(blur_loader);
    
    var blur_content = document.querySelector(".blur-content");
    blur_content.innerHTML = "";
    blur_content.appendChild(blur_loading);
}

const BlurOk = () => {
    const blur_ok = document.createElement("div");
    blur_ok.classList.add("blur-ok");

    const blur_text = document.createElement("h2");
    blur_text.textContent = "Ödeme İşlemi Başarılı";

    const blur_svg = document.createElement("svg");
    blur_svg.innerHTML = `
    <svg xmlns="http://www.w3.org/2000/svg"
         viewBox="0 0 50 50"
         width="50"
         height="50">
      <path
        d="M 25 2 C 12.317 2 2 12.317 2 25
           C 2 37.683 12.317 48 25 48
           C 37.683 48 48 37.683 48 25
           C 48 20.44 46.660281 16.189328 44.363281 12.611328
           L 42.994141 14.228516
           C 44.889141 17.382516 46 21.06 46 25
           C 46 36.579 36.579 46 25 46
           C 13.421 46 4 36.579 4 25
           C 4 13.421 13.421 4 25 4
           C 30.443 4 35.393906 6.0997656 39.128906 9.5097656
           L 40.4375 7.9648438
           C 36.3525 4.2598437 30.935 2 25 2
           z
           M 43.236328 7.7539062
           L 23.914062 30.554688
           L 15.78125 22.96875
           L 14.417969 24.431641
           L 24.083984 33.447266
           L 44.763672 9.046875
           L 43.236328 7.7539062
           z"
        fill="currentColor"
      />
    </svg>
    `;

    blur_ok.appendChild(blur_text);
    blur_ok.appendChild(blur_svg);

    const blur_content = document.querySelector(".blur-content");
    blur_content.innerHTML = "";
    blur_content.appendChild(blur_ok);
}

const BlurFailure = () => {
    const blur_failure = document.createElement("div");
    blur_failure.classList.add("blur-failure");

    const blur_text = document.createElement("h2");
    blur_text.textContent = "Ödeme İşlemi Başarısız";

    const blur_svg = document.createElement("svg");
    blur_svg.innerHTML = `
    <svg xmlns="http://www.w3.org/2000/svg"
         viewBox="0 0 50 50"
         width="50"
         height="50">
      <path d="M 9.15625 6.3125
               L 6.3125 9.15625
               L 22.15625 25
               L 6.21875 40.96875
               L 9.03125 43.78125
               L 25 27.84375
               L 40.9375 43.78125
               L 43.78125 40.9375
               L 27.84375 25
               L 43.6875 9.15625
               L 40.84375 6.3125
               L 25 22.15625 Z"
            fill="currentColor"/>
    </svg>
    `;

    blur_failure.appendChild(blur_text);
    blur_failure.appendChild(blur_svg);

    const blur_content = document.querySelector(".blur-content");
    blur_content.innerHTML = "";
    blur_content.appendChild(blur_failure);
}




const CartList = (carts) => {
    if(carts === null || carts === undefined) return;

    const cartList = Array.from(carts);
    const cartContainer = document.querySelector(".cart-container");
    cartContainer.innerHTML = "";

    const confirm_button = document.createElement("button");
    confirm_button.classList.add("cart-confirm");
    confirm_button.addEventListener("click", () => CreateOrderCommandAsync(cartList));

    
    cartList.forEach(cart => {
        const cartBox = document.createElement("div");
        cartBox.classList.add("cart-box");
        cartBox.setAttribute("data-id", cart.id);

        const cartName = document.createElement("h4");
        cartName.classList.add("cart-name");
        cartName.textContent = cart.name;

        const cartQuantity = document.createElement("h4");
        cartQuantity.classList.add("cart-quantity");
        cartQuantity.textContent = cart.quantity;

        const cartButtonLeft = document.createElement("button");
        cartButtonLeft.classList.add("cart-button-left");
        cartButtonLeft.textContent = "➕";

        const cartButtonRight = document.createElement("button");
        cartButtonRight.classList.add("cart-button-right");
        cartButtonRight.textContent = "➖";

        cartButtonLeft.addEventListener("click", (event) => AddToCart(event, cart, false));
        cartButtonRight.addEventListener("click", (event) => AddToCart(event, cart, true));

        cartBox.appendChild(cartName);
        cartBox.appendChild(cartQuantity);
        cartBox.appendChild(cartButtonLeft);
        cartBox.appendChild(cartButtonRight);

        cartContainer.appendChild(cartBox);
    });

    const totalPrice = cartList.reduce((total, cart) => total + cart.price * cart.quantity, 0);

    const price = document.querySelector(".cart-totalprice");

    if(price !== null) price.innerHTML = `Total Price : ${totalPrice.toFixed(2)}`;
    else {
        const cart_price = document.createElement("h4");
        cart_price.classList.add("cart-totalprice");
        cart_price.textContent = `Total Price : ${totalPrice.toFixed(2)}`;
        cartContainer.appendChild(cart_price);
    }
    cartContainer.appendChild(confirm_button);
}
const AddToCart = (event, product, isRemove = false) => {
    event.preventDefault();
    event.stopPropagation();
    
    var carts = localStorage.getItem("carts");
    var cartList = JSON.parse(carts);

    if(cartList === null || cartList === undefined) {
        cartList = [{ ...product, quantity: 1 }];
        localStorage.setItem("carts", JSON.stringify(cartList));
        CartList(cartList);
        return;
    }
    const selected = cartList.find(cart => cart.id === product.id);
    if(selected === undefined || selected === null){
        cartList.push({ ...product, quantity: 1 });
        localStorage.setItem("carts", JSON.stringify(cartList));
        CartList(cartList);
        return;
    }
    if(isRemove){
        selected.quantity -= 1;
        if(selected.quantity <= 0){
        cartList = cartList.filter(c => c.id !== product.id);
        }
    }
    else {
        selected.quantity += 1;
    }
    localStorage.setItem("carts", JSON.stringify(cartList));
    CartList(cartList);
    return;
}

const PollingOrderStatusAsync = async (orderId) => {
    const timeout = 60000;
    const date = Date.now();
    const polling_event = setInterval(async () => {

        const response = await FetchCheck("POST", `http://localhost:5003/api/Order/status/iyzico?orderId=${orderId}`);
        console.log(response);
        if(Date.now() - date > timeout){
            BlurOptions(BlurFailure);
            window.clearInterval(polling_event);
        }
        if(response.status === "Paid"){
            BlurOptions(BlurOk, "ok");
            clearInterval(polling_event);
            localStorage.removeItem("carts");
            
        }
        else if(response.status === "PendingPayment"){
            BlurOptions(BlurLoading, "loading");
        }
        else {
            BlurOptions(BlurFailure, "failure");
            clearInterval(polling_event);
        }
    }, 2000);
}


const CreateOrderCommandAsync = async (carts) => {
    const cart_list = carts.map(cart => ({ productId : cart.id, quantity : cart.quantity }));
    const body = {
        products : cart_list,
        currency : "TRY"
    };
    
    const response = await Order(body);
    if(response){

        window.open(response.paymentUrl, "_blank");

        BlurOptions(BlurLoading);
        
        await PollingOrderStatusAsync(response.orderId);
    }
    
}
const OnLoadEvent = async () => {

    localStorage.removeItem("idempotant_key");
    const idempotant_key = crypto.randomUUID();
    localStorage.setItem("idempotant_key", idempotant_key);

    if(!localStorage.getItem("token")) await Login();

    var payment_container = document.createElement("div");
    payment_container.classList.add("payment-container");
    
    const isCartList = document.querySelector(".cart-container");

    if(!isCartList){
        var list = localStorage.getItem("carts");

        var cart_container = document.createElement("div");
        cart_container.classList.add("cart-container");
        document.body.appendChild(cart_container);

        CartList(JSON.parse(list));
    }

    document.body.appendChild(payment_container);
    
    var products = await Products(localStorage.getItem("token"));
    
    products.forEach(product => {
        
        var payment_box = document.createElement("div");
        payment_box.classList.add("payment-box");
        payment_container.appendChild(payment_box);

        var id = document.createElement("h6");
        id.classList.add("payment-id");
        id.textContent = product.id;

        var name = document.createElement("h4");
        name.classList.add("payment-title");
        name.textContent = product.name;

        var price = document.createElement("h5");
        price.classList.add("payment-price");
        price.textContent = product.price;

        var stock = document.createElement("h5");
        stock.classList.add("payment-stock");
        stock.textContent = product.stock;

        var addButton = document.createElement("button");
        addButton.classList.add("payment-button");
        addButton.textContent = "Add to Cart";

        addButton.addEventListener("click", (event)=> AddToCart(event, product, false));

        payment_box.appendChild(id);
        payment_box.appendChild(name);
        payment_box.appendChild(price);
        payment_box.appendChild(stock);
        payment_box.appendChild(addButton);
        
    });

}
document.addEventListener("DOMContentLoaded", OnLoadEvent);

