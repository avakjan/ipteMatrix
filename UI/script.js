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

function loadSkills() {
    fetch(`${apiUrl}/skill`)
    .then(response => response.json())
    .then(data => {
        const skillCheckboxes = document.getElementById('skillCheckboxes');
        skillCheckboxes.innerHTML = '';  // Clear previous checkboxes

        // Create a checkbox for each skill
        data.forEach(skill => {
            const checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.id = `skill-${skill.id}`;
            checkbox.value = skill.id;
            checkbox.name = 'skills';

            const label = document.createElement('label');
            label.htmlFor = `skill-${skill.id}`;
            label.innerText = skill.name;

            const div = document.createElement('div');
            div.appendChild(checkbox);
            div.appendChild(label);

            skillCheckboxes.appendChild(div);
        });
    })
    .catch(error => console.error('Error loading skills:', error));
}

function registerPerson() {
    const name = document.getElementById('regName').value;
    const workplaceId = document.getElementById('regWorkplace').value;
    
    // Collect selected skills (checked checkboxes)
    const selectedSkills = Array.from(document.querySelectorAll('input[name="skills"]:checked'))
        .map(checkbox => checkbox.value);

    fetch(`${apiUrl}/person`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name, workplaceId, skillIds: selectedSkills })
    })
    .then(response => response.json())
    .then(data => alert('Person registered successfully!'))
    .catch(error => console.error('Error:', error));
}

window.onload = loadSkills;

function updatePerson() {
    const id = document.getElementById('updateId').value;
    const name = document.getElementById('updateName').value;
    const workplaceId = document.getElementById('updateWorkplace').value;
    const skillIds = document.getElementById('updateSkills').value.split(',').map(Number);

    fetch(`${apiUrl}/person/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name, workplaceId, skillIds })
    })
    .then(() => alert('Person updated successfully!'))
    .catch(error => console.error('Error:', error));
}

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