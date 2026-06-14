// Mobile menu toggle
const mobileMenuToggle = document.getElementById('mobile-menu-toggle');
const navMenu = document.getElementById('navMenu');

if (mobileMenuToggle && navMenu) {
    mobileMenuToggle.addEventListener('click', () => {
        navMenu.classList.toggle('active');
    });
}

// Sliding pill navigation effect
function initSlidingPill() {
    const PILL = document.querySelector('#navPill');
    const MENU_LINKS = document.querySelectorAll('.nav-menu .nav-item, .nav-menu a');
    const MENU_LINKS_PARENT = document.querySelector('.nav-menu');

    if (!PILL || !MENU_LINKS_PARENT || MENU_LINKS.length === 0) return;

    function getCurrentPageName() {
        const path = window.location.pathname.toLowerCase();
        if (path === '/' || path === '') return 'home';
        if (path.includes('/services')) return 'services';
        if (path.includes('/shop')) return 'shop';
        if (path.includes('/appointmentstart') || path.includes('/bookappointment') || path.includes('/appointment')) return 'appointment';
        if (path.includes('/about')) return 'about';
        if (path.includes('/contact')) return 'contact';
        if (path.includes('/help')) return 'help';
        return null;
    }

    function getPageNameFromHref(href) {
        if (!href) return null;
        const h = href.toLowerCase();
        if (h === '/' || h === '') return 'home';
        if (h.includes('services')) return 'services';
        if (h.includes('shop')) return 'shop';
        if (h.includes('appointment')) return 'appointment';
        if (h.includes('about')) return 'about';
        if (h.includes('contact')) return 'contact';
        if (h.includes('help')) return 'help';
        return null;
    }

    function setPill() {
        let activeItem = null;
        const currentPage = getCurrentPageName();
        const currentPath = window.location.pathname.toLowerCase();

        MENU_LINKS.forEach((e) => {
            if (e.classList.contains('active')) activeItem = e;
        });

        if (!activeItem) {
            MENU_LINKS.forEach((e) => {
                const href = e.getAttribute('href') || '';
                const pageName = getPageNameFromHref(href);
                let isMatch = pageName && pageName === currentPage;
                if (currentPage === 'appointment' && pageName === 'appointment') isMatch = true;
                if (currentPath.includes('bookappointment') && href.toLowerCase().includes('appointment')) isMatch = true;
                if (isMatch) { activeItem = e; e.classList.add('active'); }
            });
        }

        if (!activeItem && currentPage === 'appointment') {
            MENU_LINKS.forEach((e) => {
                if (e.getAttribute('href')?.toLowerCase().includes('appointment')) {
                    activeItem = e; e.classList.add('active');
                }
            });
        }

        if (!activeItem && currentPage === 'home' && MENU_LINKS.length > 0) {
            activeItem = MENU_LINKS[0]; activeItem.classList.add('active');
        }

        if (activeItem) {
            const dims = activeItem.getBoundingClientRect();
            const parentRect = MENU_LINKS_PARENT.getBoundingClientRect();
            if (dims.width > 0 && dims.height > 0) {
                PILL.style.width = dims.width + 'px';
                PILL.style.height = dims.height + 'px';
                PILL.style.left = (dims.left - parentRect.left) + 'px';
                PILL.style.opacity = '1';
                PILL.style.visibility = 'visible';
            } else {
                setTimeout(setPill, 50);
            }
        } else {
            PILL.style.opacity = '0';
            PILL.style.visibility = 'hidden';
        }
    }

    // Set initial pill position - wait for full page load
    function initializePill() {
        // Multiple attempts to ensure it works - wait for everything to be ready
        setTimeout(() => {
            setPill();
            setTimeout(setPill, 50);
            setTimeout(setPill, 150);
            setTimeout(setPill, 300);
            setTimeout(setPill, 500);
        }, 100);
    }

    // Initialize on multiple events to ensure it works
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initializePill);
    } else {
        initializePill();
    }
    
    window.addEventListener('load', initializePill);
    
    // Also try after a short delay to catch any late-rendering
    setTimeout(initializePill, 1000);
    
    // Update on window resize
    let resizeTimeout;
    window.addEventListener('resize', () => {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(setPill, 100);
    });
    
    // Update on navigation clicks
    MENU_LINKS.forEach((e) => {
        e.addEventListener('click', () => {
            // Remove active from all
            MENU_LINKS.forEach(link => link.classList.remove('active'));
            // Add active to clicked
            e.classList.add('active');
            setTimeout(setPill, 50);
        });
    });
}

// Initialize sliding pill
initSlidingPill();

// Smooth scrolling for navigation links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        var href = this.getAttribute('href');
        if (!href || href === '#') return;
        e.preventDefault();
        var target = document.querySelector(href);
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Subtle shadow enhancement on scroll for fixed header
window.addEventListener('scroll', () => {
    const header = document.querySelector('.header');
    if (!header) return;
    header.style.boxShadow = window.scrollY > 50
        ? '0 4px 20px rgba(0, 0, 0, 0.1)'
        : '0 2px 16px rgba(0, 0, 0, 0.08)';
});

// Animate hero content on load
window.addEventListener('load', () => {
    const heroContent = document.querySelector('.hero-content');
    if (heroContent) {
        heroContent.style.transform = 'translateY(0)';
    }
});

// Add click handlers for CTA buttons (you'll replace these with actual functionality)
document.querySelectorAll('[href="#book-appointment"]').forEach(button => {
    button.addEventListener('click', (e) => {
        e.preventDefault();
        alert('Redirect to booking page - integrate with your ASP.NET appointment booking system');
        // Replace with: window.location.href = '/Appointments/Book';
    });
});

document.querySelectorAll('[href="#shop"]').forEach(button => {
    button.addEventListener('click', (e) => {
        e.preventDefault();
        alert('Redirect to shop page - integrate with your ASP.NET product catalog');
        // Replace with: window.location.href = '/Shop';
    });
});