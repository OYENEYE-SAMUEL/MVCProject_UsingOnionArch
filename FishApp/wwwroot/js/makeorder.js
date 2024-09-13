"use strict";
var index = document.querySelector('#index');
const viewBag = document.querySelector("#viewBag").textContent;
const fishList = JSON.parse(viewBag);

function addNewRow() {
    const selectedId = document.querySelector('#fish-select').value;
    const quantity = document.querySelector('#fish-quantity');
    var table = document.getElementById('order-fish-items');
    var row = table.insertRow();
    let count = parseInt(index.textContent);

    var selectedFish = fishList.find(fish => fish.Name == selectedId);
    console.log(count);

    // Assuming selectedFish.ImageUrl contains the fish image path
    row.innerHTML = `
    <td><img src="/Uploads/${selectedFish.ImageUrl}" alt="Fish Image" class="fish-image" /></td>
    <td>
        <div class="fish-item">
            <div class="fish-details">
                <p>${selectedFish.Name}</p>
                <p>Price: #${selectedFish.Price}</p>
                <p>Available: ${selectedFish.Quantity}</p>
                <input name="OrderItems[${count}].Key" value="${selectedId}" type="hidden" class="form-control fish-select">
            </div>
        </div>
    </td>
    <td>
        <div class="fish-actions">
            <input name="OrderItems[${count}].Value" value="${quantity.value}" class="form-control fish-quantity-input" type="number" min="1" />
        </div>
    </td>
    <td>
        <button type="button" class="btn btn-danger" onclick="removeRow(this)">Remove</button>
    </td>`;

    index.textContent = count + 1;
    quantity.value = 1;
    console.log(index.textContent);
}

function removeRow(button) {
    var row = button.closest('tr');
    row.parentNode.removeChild(row);
}
