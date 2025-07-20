// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// 파티클 효과 생성
function createParticles() {
    const particlesContainer = document.getElementById('particles');

    for (let i = 0; i < 50; i++) {
        const particle = document.createElement('div');
        particle.className = 'particle';
        particle.style.left = Math.random() * 100 + '%';
        particle.style.animationDelay = Math.random() * 8 + 's';
        particle.style.animationDuration = (Math.random() * 6 + 6) + 's';
        particlesContainer.appendChild(particle);
    }
}

// 마우스 움직임에 따른 스포트라이트 효과
document.addEventListener('mousemove', (e) => {
    const spotlights = document.querySelectorAll('.spotlight');
    const x = e.clientX / window.innerWidth;
    const y = e.clientY / window.innerHeight;

    spotlights.forEach((spotlight, index) => {
        const moveX = (x - 0.5) * 50 * (index + 1);
        const moveY = (y - 0.5) * 30 * (index + 1);
        spotlight.style.transform = `translate(${moveX}px, ${moveY}px)`;
    });
});

// 카드 호버 효과 강화
document.querySelectorAll('.feature-card').forEach(card => {
    card.addEventListener('mouseenter', () => {
        card.style.boxShadow = '0 25px 50px rgba(255, 215, 0, 0.3)';
        card.style.borderColor = 'rgba(255, 215, 0, 0.5)';
    });

    card.addEventListener('mouseleave', () => {
        card.style.boxShadow = 'none';
        card.style.borderColor = 'rgba(255, 255, 255, 0.2)';
    });
});

// 페이지 로드 시 파티클 생성
createParticles();