window.auth = {
    loginAndReload: async function (loginData) {
        try {
            const response = await fetch("/api/Auth/Login", {
                method: "POST",
                credentials: "include", // Send cookies
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(loginData)
            });

            if (response.ok) {
                location.reload();
            } else {
                const err = await response.text();
                console.error("Login failed", err);
                alert("Login failed");
            }
        } catch (e) {
            console.error("Network or server error", e);
        }
    },
    registerUser: async function (registrationData) {
    const response = await fetch('/api/Auth/Register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            username: registrationData.username,
            email: registrationData.email,
            password: registrationData.password,
            confirmPassword: registrationData.confirmPassword
        })
    });

    if (response.ok) {
        // Reload to start a new authenticated circuit
        location.reload();
    } else {
        const error = await response.text();
        console.error('Registration failed:', error);
        alert('Registration failed: ' + error);
    }
}
};