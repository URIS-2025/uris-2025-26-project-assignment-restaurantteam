function Footer() {
  return (
    <footer style={{
      backgroundColor: '#1a1a1a',
      borderTop: '2px solid #c9a84c',
      padding: '48px 0 24px',
      marginTop: '80px'
    }}>
      <div className="container">
        <div className="row g-4 mb-4">

          {/* O nama */}
          <div className="col-md-4">
            <h5 style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', marginBottom: '16px', fontSize: '1.2rem' }}>
              🍽️ Restaurant
            </h5>
            <p style={{ color: '#9b9080', fontSize: '0.9rem', lineHeight: '1.8' }}>
              Autentičan ukus domaće kuhinje. Sveži sastojci, ljubav u svakom zalogaju.
            </p>
          </div>

          {/* Linkovi */}
          <div className="col-md-4">
            <h5 style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', marginBottom: '16px', fontSize: '1.1rem', letterSpacing: '1px' }}>
              NAVIGACIJA
            </h5>
            <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
              {[
                { label: 'Početna', href: '/' },
                { label: 'Meni', href: '/menu' },
                { label: 'Rezervacije', href: '/reservations' },
                { label: 'Narudžbine', href: '/orders' }
              ].map((link, i) => (
                <a key={i} href={link.href} style={{
                  color: '#9b9080',
                  textDecoration: 'none',
                  fontSize: '0.9rem',
                  transition: 'color 0.2s'
                }}
                  onMouseEnter={e => e.target.style.color = '#c9a84c'}
                  onMouseLeave={e => e.target.style.color = '#9b9080'}>
                  {link.label}
                </a>
              ))}
            </div>
          </div>

          {/* Kontakt */}
          <div className="col-md-4">
            <h5 style={{ color: '#c9a84c', fontFamily: 'Georgia, serif', marginBottom: '16px', fontSize: '1.1rem', letterSpacing: '1px' }}>
              KONTAKT
            </h5>
            <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
              {[
                { icon: '📍', text: 'Ulica bb, Grad, Srbija' },
                { icon: '📞', text: '+381 xx xxx xxxx' },
                { icon: '✉️', text: 'restoran@email.com' },
                { icon: '🕐', text: 'Pon-Pet: 08:00 - 23:00' }
              ].map((item, i) => (
                <p key={i} style={{ color: '#9b9080', fontSize: '0.9rem', margin: 0 }}>
                  {item.icon} {item.text}
                </p>
              ))}
            </div>
          </div>
        </div>

        {/* Divider */}
        <div style={{ borderTop: '1px solid #2c2c2c', paddingTop: '24px', textAlign: 'center' }}>
          <p style={{ color: '#6b6457', fontSize: '0.85rem', margin: 0 }}>
            © 2026 Restaurant App. Sva prava zadržana.
          </p>
        </div>
      </div>
    </footer>
  )
}

export default Footer