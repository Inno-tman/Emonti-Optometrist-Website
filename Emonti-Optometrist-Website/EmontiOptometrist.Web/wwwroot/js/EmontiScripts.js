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

    // Helper function to get current page name from URL
    function getCurrentPageName() {
        const path = window.location.pathname.toLowerCase();
        
        // Check for specific pages first (more specific matches)
        if (path.includes('/services.aspx') || (path.includes('/services') && !path.includes('/services/'))) return 'services';
        if (path.includes('/shop.aspx') || (path.includes('/shop') && !path.includes('/shop/'))) return 'shop';
        if (path.includes('/appointmentstart.aspx') || path.includes('/bookappointment.aspx') || path.includes('/appointment')) return 'appointment';
        if (path.includes('/about.aspx') || (path.includes('/about') && !path.includes('/about/'))) return 'about';
        if (path.includes('/contact.aspx') || (path.includes('/contact') && !path.includes('/contact/'))) return 'contact';
        if (path.includes('/help.aspx') || (path.includes('/help') && !path.includes('/help/'))) return 'help';
        
        // Check for home page (default page)
        if (path === '/' || 
            path === '/default.aspx' ||
            path.endsWith('/default.aspx') ||
            (path.endsWith('/') && path.length <= 2)) {
            return 'home';
        }
        
        return null;
    }

    // Helper function to get page name from href
    function getPageNameFromHref(href) {
        if (!href) return null;
        const hrefLower = href.toLowerCase();
        // Check for home/default first
        if (hrefLower.includes('default') || 
            hrefLower === '~/' || 
            hrefLower === '/' ||
            hrefLower.endsWith('/default.aspx') ||
            (hrefLower.includes('~') && !hrefLower.includes('services') && 
             !hrefLower.includes('shop') && !hrefLower.includes('appointment') &&
             !hrefLower.includes('about') && !hrefLower.includes('contact') &&
             !hrefLower.includes('help'))) {
            return 'home';
        }
        if (hrefLower.includes('services')) return 'services';
        if (hrefLower.includes('shop')) return 'shop';
        if (hrefLower.includes('appointment')) return 'appointment';
        if (hrefLower.includes('about')) return 'about';
        if (hrefLower.includes('contact')) return 'contact';
        if (hrefLower.includes('help')) return 'help';
        return null;
    }

    function setPill() {
        let activeItem = null;
        const currentPage = getCurrentPageName();
        const currentPath = window.location.pathname.toLowerCase();
        
        // First, try to find item with active class (set by code-behind)
        MENU_LINKS.forEach((e) => {
            if (e.classList.contains('active')) {
                activeItem = e;
            }
        });
        
        // If no active class found, try to match by URL (only for pages in navigation)
        if (!activeItem) {
            MENU_LINKS.forEach((e) => {
                const href = e.getAttribute('href') || '';
                const hrefLower = href.toLowerCase();
                const pageName = getPageNameFromHref(href);
                
                // Check multiple conditions for matching
                let isMatch = false;
                
                // Match by page name (both should return 'appointment' for appointment pages)
                if (pageName && pageName === currentPage) {
                    isMatch = true;
                }
                
                // Special handling for appointment pages - match BookAppointment with AppointmentStart link
                // Both BookAppointment.aspx and AppointmentStart.aspx should match the appointment nav item
                if (currentPage === 'appointment' && (pageName === 'appointment' || hrefLower.includes('appointment'))) {
                    isMatch = true;
                }
                
                // Also check if current path matches the href directly (for resolved ASP.NET paths)
                // Handle both ~/ paths and resolved paths
                let hrefPath = hrefLower.replace('~/', '/').replace('.aspx', '');
                if (hrefPath && currentPath.includes(hrefPath)) {
                    isMatch = true;
                }
                
                // For appointment pages specifically, check if current path contains 'bookappointment' 
                // and href contains 'appointment' (to match BookAppointment with AppointmentStart link)
                if (currentPath.includes('bookappointment') && hrefLower.includes('appointment')) {
                    isMatch = true;
                }
                
                // Special handling for home page
                if (pageName === 'home' && (currentPath === '/' || currentPath === '/default.aspx' || currentPath.endsWith('/default.aspx') || (currentPath.endsWith('/') && currentPath.length <= 2))) {
                    isMatch = true;
                }
                
                // Match by href pattern
                if (hrefLower.includes('default') && (currentPath === '/' || currentPath.includes('default'))) {
                    isMatch = true;
                }
                
                if (isMatch) {
                    activeItem = e;
                    // Also add active class for consistency
                    e.classList.add('active');
                }
            });
        }
        
        // Fallback: if on appointment page and no active found, find appointment nav item by ID or href
        if (!activeItem && currentPage === 'appointment') {
            MENU_LINKS.forEach((e) => {
                const href = e.getAttribute('href') || '';
                const hrefLower = href.toLowerCase();
                const id = e.getAttribute('id') || '';
                const idLower = id.toLowerCase();
                
                // Check if this is the appointment nav item (by ID or href containing appointment)
                if (idLower.includes('appointment') || hrefLower.includes('appointment')) {
                    activeItem = e;
                    e.classList.add('active');
                }
            });
        }
        
        // Fallback: if on home page and no active found, use first item (Home)
        if (!activeItem && currentPage === 'home' && MENU_LINKS.length > 0) {
            activeItem = MENU_LINKS[0];
            activeItem.classList.add('active');
        }
        
        // Only position the pill if we found an active item
        // For pages not in navigation (Cart, Login, Register, etc.), don't show pill on any item
        if (activeItem) {
            const dimensions = activeItem.getBoundingClientRect();
            const parentRect = MENU_LINKS_PARENT.getBoundingClientRect();
            
            if (dimensions.width > 0 && dimensions.height > 0) {
                PILL.style.width = dimensions.width + 'px';
                PILL.style.height = dimensions.height + 'px';
                PILL.style.left = (dimensions.left - parentRect.left) + 'px';
                PILL.style.opacity = '1';
                PILL.style.visibility = 'visible';
            } else {
                // Retry if dimensions aren't ready yet
                setTimeout(setPill, 50);
            }
        } else {
            // Hide the pill if no active item (for pages not in main navigation)
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

// Dynamic floating pill nav bar effect on scroll (Stitch.money style)
window.addEventListener('scroll', () => {
    const header = document.querySelector('.header');
    const scrollY = window.scrollY;
    
    if (scrollY > 50) {
        // More opaque and stronger shadow when scrolled (floating pill effect)
        header.style.background = 'rgba(255, 255, 255, 0.95)';
        header.style.boxShadow = '0 12px 40px rgba(0, 0, 0, 0.15), 0 6px 20px rgba(0, 0, 0, 0.1)';
        header.style.backdropFilter = 'blur(24px) saturate(180%)';
        header.style.webkitBackdropFilter = 'blur(24px) saturate(180%)';
        header.style.top = '0.75rem';
    } else {
        // Lighter, more transparent when at top (floating pill above content)
        header.style.background = 'rgba(255, 255, 255, 0.85)';
        header.style.boxShadow = '0 8px 32px rgba(0, 0, 0, 0.12), 0 4px 16px rgba(0, 0, 0, 0.08)';
        header.style.backdropFilter = 'blur(20px) saturate(180%)';
        header.style.webkitBackdropFilter = 'blur(20px) saturate(180%)';
        header.style.top = '1rem';
    }
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