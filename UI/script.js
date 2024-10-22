const apiUrl = 'http://localhost:5117/api';

let adminToken = '';

function adminLogin() {
    const username = document.getElementById('adminUsername').value;
    const password = document.getElementById('adminPassword').value;

    fetch('http://localhost:5117/api/auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username, password })
    })
    .then(response => response.json())
    .then(data => {
        if (data.token) {
            adminToken = data.token;
            document.getElementById('loginMessage').innerText = 'Logged in successfully';

            // Safely check if adminSkillForm exists before accessing it
            const adminSkillForm = document.getElementById('adminSkillForm');
            if (adminSkillForm) {
                adminSkillForm.style.display = 'block';
            }
            const adminWorkplaceForm = document.getElementById('adminWorkplaceForm');
            if (adminWorkplaceForm) {
                adminWorkplaceForm.style.display = 'block';
            }
        } else {
            document.getElementById('loginMessage').innerText = 'Login failed';
        }
    })
    .catch(error => {
        document.getElementById('loginMessage').innerText = 'Login failed';
        console.error('Error:', error);
    });
}

function addSkill() {
    const skillName = document.getElementById('newSkillName').value;

    fetch('http://localhost:5117/api/skill', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${adminToken}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: skillName })
    })
    .then(response => response.json())
    .then(data => alert('Skill added successfully!'))
    .catch(error => console.error('Error:', error));
}

function addWorkplace() {
    const workplaceName = document.getElementById('newWorkplaceName').value;

    fetch('http://localhost:5117/api/workplace', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${adminToken}`, // Ensure adminToken is available
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: workplaceName })
    })
    .then(response => response.json())
    .then(data => alert('Workplace added successfully!'))
    .catch(error => console.error('Error:', error));
}


function loadSkillsForAddPerson() {
    fetch(`${apiUrl}/skill`)
    .then(response => response.json())
    .then(data => {
        const skillCheckboxes = document.getElementById('skillCheckboxes');
        skillCheckboxes.innerHTML = '';  // Clear previous checkboxes

        data.forEach(skill => {
            const div = document.createElement('div');

            // Create the skill checkbox
            const checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.id = `skill-${skill.id}`;
            checkbox.value = skill.id;
            checkbox.name = 'skills';

            const label = document.createElement('label');
            label.htmlFor = `skill-${skill.id}`;
            label.innerText = skill.name;

            // Create a level dropdown for the skill
            const levelSelect = document.createElement('select');
            levelSelect.id = `level-${skill.id}`;
            levelSelect.disabled = true; // Disable initially

            // Add options for level 1-5
            for (let i = 1; i <= 5; i++) {
                const option = document.createElement('option');
                option.value = i;
                option.text = `Level ${i}`;
                levelSelect.appendChild(option);
            }

            // Enable/disable level dropdown when checkbox is checked/unchecked
            checkbox.addEventListener('change', function() {
                levelSelect.disabled = !checkbox.checked;
            });

            div.appendChild(checkbox);
            div.appendChild(label);
            div.appendChild(levelSelect);
            skillCheckboxes.appendChild(div);
        });
    })
    .catch(error => console.error('Error loading skills:', error));
}

let workplaces = []; // Array to hold workplace data

// Call this function to populate skills when the update form loads
// Load all skills, check the person's skills, and set their levels
function loadSkillsForUpdatePerson(personSkills, personLevels) {
    fetch(`${apiUrl}/skill`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${adminToken}`
        }
    })
    .then(response => response.json())
    .then(allSkills => {
        const skillCheckboxes = document.getElementById('updateSkillCheckboxes');
        skillCheckboxes.innerHTML = '';  // Clear previous checkboxes

        allSkills.forEach((skill, index) => {
            const div = document.createElement('div');

            // Create the skill checkbox
            const checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.id = `update-skill-${skill.id}`;
            checkbox.value = skill.id;
            checkbox.name = 'skills';

            const label = document.createElement('label');
            label.htmlFor = `update-skill-${skill.id}`;
            label.innerText = skill.name;

            // Create a level dropdown for the skill
            const levelSelect = document.createElement('select');
            levelSelect.id = `update-level-${skill.id}`;
            levelSelect.disabled = true; // Disable initially

            // Add options for level 1-5
            for (let i = 1; i <= 5; i++) {
                const option = document.createElement('option');
                option.value = i;
                option.text = `Level ${i}`;
                levelSelect.appendChild(option);
            }

            // Check if the person has this skill and set the level
            const skillIndex = personSkills.indexOf(skill.name); // Check if person has this skill
            if (skillIndex !== -1) {
                checkbox.checked = true;
                levelSelect.disabled = false;  // Enable the level dropdown
                levelSelect.value = personLevels[skillIndex];  // Set the correct level
            }

            // Enable/disable level dropdown when checkbox is checked/unchecked
            checkbox.addEventListener('change', function() {
                levelSelect.disabled = !checkbox.checked;
            });

            div.appendChild(checkbox);
            div.appendChild(label);
            div.appendChild(levelSelect);
            skillCheckboxes.appendChild(div);
        });
    })
    .catch(error => console.error('Error loading skills:', error));
}

// Function to register a new person
function registerPerson() {
    const name = document.getElementById('regName').value;
    const workplaceId = parseInt(document.getElementById('regWorkplace').value);

    const selectedSkills = [];
    document.querySelectorAll('#skillCheckboxes input[type="checkbox"]:checked').forEach(checkbox => {
        const skillId = parseInt(checkbox.value);
        const level = parseInt(document.getElementById(`level-${skillId}`).value); // Get selected level
        selectedSkills.push({ skillId, level });
    });

    fetch(`${apiUrl}/person/add`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${adminToken}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            id: 0, // Always 0 for a new person
            name: name,
            workplaceId: workplaceId,
            skillLevels: selectedSkills // Send skill IDs and levels
        })
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(text => { throw new Error(text) });
        }
        return response.text(); // Handle plain text response
    })
    .then(data => {
        alert(data); // Show the plain text message, e.g., "Person Gabriel added successfully"
    })
    .catch(error => {
        console.error('Error registering person:', error);
        alert(`Error registering person: ${error.message}`);
    });
}

//TODO
function updatePerson() {
    const id = parseInt(document.getElementById('updateId').value);
    const name = document.getElementById('updateName').value;
    const workplaceId = parseInt(document.getElementById('updateWorkplace').value);

    const selectedSkills = [];
    
    // Iterate through checked checkboxes
    document.querySelectorAll('#updateSkillCheckboxes input[type="checkbox"]:checked').forEach(checkbox => {
        const skillId = parseInt(checkbox.value);
        console.log(`Processing skill ID: ${skillId}`);  // Debugging step to check skill ID
        
        // Get the corresponding level dropdown
        const levelSelect = document.getElementById(`update-level-${skillId}`);
        console.log(`Level select element:`, levelSelect);  // Check if the element exists

        // Ensure that the levelSelect exists before trying to access its value
        if (levelSelect && !levelSelect.disabled) {
            const level = parseInt(levelSelect.value);  // Get selected level
            console.log(`Selected level for skill ID ${skillId}: ${level}`);  // Debugging step for level
            selectedSkills.push({ skillId, level });
        } else {
            console.warn(`No level select found for skill ID ${skillId}, or it is disabled.`);
        }
    });

    // If selectedSkills is empty, show an error and exit the function
    if (selectedSkills.length === 0) {
        alert('Please select at least one skill and level.');
        return;
    }

    fetch(`${apiUrl}/person/${id}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${adminToken}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            name: name,
            workplaceId: workplaceId,
            skillLevels: selectedSkills // Send updated skill IDs and levels
        })
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(text => { throw new Error(text) });
        }
        return response.json();
    })
    .then(data => {
        alert('Person updated successfully!');
    })
    .catch(error => {
        console.error('Error updating person:', error);
        alert(`Error updating person: ${error.message}`);
    });
}



function loadWorkplacesForUpdatePerson() {
    fetch(`${apiUrl}/workplace`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${adminToken}`
        }
    })
    .then(response => response.json())
    .then(data => {
        const workplaceSelect = document.getElementById('updateWorkplace');
        workplaceSelect.innerHTML = '';  // Clear previous options
        data.forEach(workplace => {
            const option = document.createElement('option');
            option.value = workplace.id;
            option.textContent = workplace.name;
            workplaceSelect.appendChild(option);
        });
    })
    .catch(error => console.error('Error loading workplaces:', error));
}

// Call this function to load workplaces when the page loads
loadWorkplacesForUpdatePerson();

function searchPerson() {
    const skill = document.getElementById('searchSkill').value;
    const encodedSkill = encodeURIComponent(skill); // URL encode the skill name

    fetch(`${apiUrl}/person/skill/${encodedSkill}`)
    .then(response => {
        if (!response.ok) {
            throw new Error('No persons found');
        }
        return response.json();
    })
    .then(data => {
        const results = data.map(person => `<p>${person.name} - Workplace: ${person.workplace} - Skills: ${person.skills.join(', ')}</p>`).join('');
        document.getElementById('searchResults').innerHTML = results;
    })
    .catch(error => {
        document.getElementById('searchResults').innerHTML = `<p>${error.message}</p>`;
        console.error('Error:', error);
    });
}

function searchWorkplace() {
    const skill = document.getElementById('searchWorkplaceSkill').value;
    const encodedSkill = encodeURIComponent(skill); // URL encode the skill name

    fetch(`${apiUrl}/workplace/skill/${encodedSkill}`)
    .then(response => {
        if (!response.ok) {
            throw new Error('No workplaces found');
        }
        return response.json();
    })
    .then(data => {
        const results = data.map(workplace => 
            `<p>${workplace.workplaceName} - People Count: ${workplace.peopleCount}</p>`
        ).join('');
        document.getElementById('workplaceResults').innerHTML = results;
    })
    .catch(error => {
        document.getElementById('workplaceResults').innerHTML = `<p>${error.message}</p>`;
        console.error('Error:', error);
    });
}

function loadPersons() {
    fetch(`${apiUrl}/person`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${adminToken}`
        }
    })
    .then(response => response.json())
    .then(data => {
        const personSelect = document.getElementById('updatePersonSelect');
        personSelect.innerHTML = '<option value="">Select a person</option>';  // Default option

        data.forEach(person => {
            const option = document.createElement('option');
            option.value = person.id;  // Use person's ID as the value
            option.textContent = person.name;  // Display person's name in the dropdown
            personSelect.appendChild(option);
        });
    })
    .catch(error => console.error('Error loading persons:', error));
}

// Load the selected person's details (name, workplace, and skills)
function loadPersonDetails() {
    const personId = document.getElementById('updatePersonSelect').value;
    if (!personId) return;  // If no person selected, do nothing

    // Fetch the selected person's details
    fetch(`${apiUrl}/person/${personId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${adminToken}`
        }
    })
    .then(response => response.json())
    .then(person => {
        // Populate name and workplace
        document.getElementById('updateName').value = person.name;
        loadWorkplacesForUpdatePerson(person.workplaceId);  // Load workplaces and set selected workplace
        loadSkillsForUpdatePerson(person.skills, person.levels);  // Load skills and set selected skills/levels
    })
    .catch(error => console.error('Error loading person details:', error));
}

function loadWorkplaces() {
    fetch(`${apiUrl}/workplace`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${adminToken}`  // Ensure the token is available
        }
    })
    .then(response => response.json())
    .then(data => {
        const workplaceSelect = document.getElementById('regWorkplace');
        workplaceSelect.innerHTML = '';  // Clear previous options
        data.forEach(workplace => {
            const option = document.createElement('option');
            option.value = workplace.id;  // Set the value to workplace ID
            option.textContent = workplace.name;  // Display workplace name
            workplaceSelect.appendChild(option);
        });
    })
    .catch(error => console.error('Error loading workplaces:', error));
}

window.onload = function() {
    loadPersons();  // Load person list into the dropdown
    loadSkillsForAddPerson();  // Load skills for adding a new person
    loadWorkplaces();  // Load workplaces
};