// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// Builds and returns a tag component
const buildTag = (tag) => `<div class="tag tag-suggestion" id=${tag.id}>${tag.name}</div>`



// Adds event listeners to X buttons on tags. When you click the X, we send a DELETE request to the server
document.querySelectorAll(".delete-tag").forEach(deleteBtn => {
    deleteBtn.addEventListener("click", () => {
        fetch(`/Tags/DeleteTag/${event.target.id}`, {
            method: 'DELETE'
        }).then(() => { window.location.reload(true) })
    })
})




const autoCompleteContainer = document.querySelector("#autocomplete");
let tabCounter = 0;
let selectedTag;



document.querySelector("body").addEventListener("keydown", () => {
    // As the user types, we want to show a list of suggested tags based on what they're typing
    if (event.target.classList.contains("tag-input")) {

        // Show the dropdown from autocomplete tags
        autoCompleteContainer.classList.remove("hidden");

        // If they type nothing into the search bar, or delete what they previously typed, we want the autocomplete container to be empty
        if (event.target.value === "") {
            autoCompleteContainer.innerHTML = ""

        } else if (event.keyCode === 9) { // If they press tab, move through the tags and select the next tag
            event.preventDefault();
            if (autoCompleteContainer.hasChildNodes()) {
                if (selectedTag != null) {
                    selectedTag.classList.remove("selected-tag");
                }

                selectedTag = autoCompleteContainer.childNodes[tabCounter];
                selectedTag.classList.add("selected-tag");
                tabCounter++;
            }
        } else if (event.keyCode == 13) {
            // When they press enter, we want to prevent the default submit behavior
            event.preventDefault();

            // And then determine if they've tab selected a tag or not. If they have, we'll send that tag to the server. If not, we'll send the input to the server.

            let tagName = selectedTag ? selectedTag.innerText : event.target.value


            // Once we've determined WHAT needs to go to the server, we make the POST request
            fetch(`/Tags/AddTag/${event.target.id}`, {
                method: 'POST',
                body: JSON.stringify(tagName),
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(() => {
                window.location.reload(); // TODO: refactor 
            })

        } else {
            fetch(`/Tags/ListTags?q=${event.target.value}`)
                .then(r => r.json())
                .then(tags => {
                    // Clear the tag suggestion container
                    autoCompleteContainer.innerHTML = ""
                    // Loop over the matching tags and print them to the DOM
                    tags.forEach(tag => {
                        const newTag = buildTag(tag);
                        autoCompleteContainer.innerHTML += newTag

                    })
                })

        }

    }  

})














