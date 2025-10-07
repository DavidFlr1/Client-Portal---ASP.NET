interface LoginRequest {
    username: string;
    password: string;
}

interface LoginResponse {
    token: string;
    userId: string;
    email: string;
    fullName: string;
    expiresAt: string;
}

interface ApiError {
    error: string;
    details?: string[];
    message?: string;
}

class AuthService {
    private readonly apiBaseUrl = 'http://localhost:5264/api';

    async login(credentials: LoginRequest): Promise<LoginResponse> {
        const response = await fetch(`${this.apiBaseUrl}/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(credentials)
        });

        if (!response.ok) {
            const error: ApiError = await response.json();
            throw new Error(error.error || 'Login failed');
        }

        return await response.json();
    }

    saveToken(token: string): void {
        localStorage.setItem('authToken', token);
    }

    getToken(): string | null {
        return localStorage.getItem('authToken');
    }

    removeToken(): void {
        localStorage.removeItem('authToken');
    }
}

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    const authService = new AuthService();
    const loginForm = document.getElementById('loginForm') as HTMLFormElement;
    const loginBtn = document.getElementById('loginBtn') as HTMLButtonElement;
    const loading = loginBtn.querySelector('.loading') as HTMLElement;
    const alertContainer = document.getElementById('alertContainer') as HTMLElement;

    loginForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        
        const username = (document.getElementById('username') as HTMLInputElement).value;
        const password = (document.getElementById('password') as HTMLInputElement).value;

        // Show loading state
        loginBtn.disabled = true;
        loading.style.display = 'inline-block';
        alertContainer.innerHTML = '';

        try {
            const response = await authService.login({ username, password });
            
            // Save token
            authService.saveToken(response.token);
            
            // Show success message
            showAlert('Login successful! Redirecting...', 'success');
            
            // Redirect after short delay
            setTimeout(() => {
                window.location.href = '/';
            }, 1500);
            
        } catch (error) {
            showAlert(error instanceof Error ? error.message : 'Login failed', 'danger');
        } finally {
            // Hide loading state
            loginBtn.disabled = false;
            loading.style.display = 'none';
        }
    });

    function showAlert(message: string, type: string): void {
        alertContainer.innerHTML = `
            <div class="alert alert-${type} alert-dismissible fade show" role="alert">
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `;
    }
});