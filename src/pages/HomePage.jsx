import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

function HomePage() {
  const { token } = useAuth()
  const navigate = useNavigate()

  const handleReservationClick = () => {
    if (token) {
      navigate('/reservations')
    } else {
      navigate('/login', { state: { from: '/reservations' } })
    }
  }

  return (
    <div>
      {/* Hero sekcija */}
      <div style={{
        background: 'linear-gradient(135deg, #1a1a1a 0%, #2c2c2c 100%)',
        borderRadius: '12px',
        padding: '80px 40px',
        textAlign: 'center',
        marginBottom: '40px',
        border: '2px solid #c9a84c'
      }}>
        <p style={{ color: '#c9a84c', letterSpacing: '4px', fontSize: '0.9rem', marginBottom: '16px' }}>
          DOBRODOŠLI
        </p>
        <h1 style={{
          color: '#f5f0e8',
          fontSize: '3.5rem',
          fontWeight: 'bold',
          marginBottom: '20px',
          fontFamily: 'Georgia, serif'
        }}>
          🍽️ Naš Restoran
        </h1>
        <p style={{ color: '#ede8dc', fontSize: '1.2rem', maxWidth: '600px', margin: '0 auto 36px' }}>
          Uživajte u autentičnim ukusima domaće kuhinje. Sveži sastojci, ljubav u svakom zalogaju.
        </p>
        <div>
          <button onClick={() => navigate('/menu')} style={{
            backgroundColor: '#c9a84c',
            color: '#1a1a1a',
            border: 'none',
            padding: '14px 36px',
            borderRadius: '6px',
            fontSize: '1rem',
            fontWeight: 'bold',
            marginRight: '16px',
            cursor: 'pointer'
          }}>
            Pogledaj Meni
          </button>
          <button onClick={() => navigate('/register')} style={{
            backgroundColor: 'transparent',
            color: '#c9a84c',
            border: '2px solid #c9a84c',
            padding: '14px 36px',
            borderRadius: '6px',
            fontSize: '1rem',
            fontWeight: 'bold',
            cursor: 'pointer'
          }}>
            Registruj se
          </button>
        </div>
      </div>

      {/* Opis restorana */}
      <div style={{
        backgroundColor: '#fff9f0',
        border: '1px solid #e8c96d',
        borderRadius: '12px',
        padding: '48px',
        marginBottom: '40px'
      }}>
        <h2 style={{ color: '#1a1a1a', textAlign: 'center', marginBottom: '32px', borderBottom: '2px solid #c9a84c', paddingBottom: '16px' }}>
          O našem restoranu
        </h2>
        <p style={{ color: '#2c2c2c', lineHeight: '1.9', fontSize: '1.05rem' }}>
          Naš restoran osnovan je sa jednom misijom — doneti autentičan ukus domaće kuhinje
          na vaš tanjir. Svaki dan naši kuvari sa godinama iskustva pripremaju jela od najsvežijih
          lokalnih namirnica, birajući samo ono najbolje što nam sezona nudi. Kod nas nema kompromisa
          kada je kvalitet u pitanju.
        </p>
        <p style={{ color: '#2c2c2c', lineHeight: '1.9', fontSize: '1.05rem', marginTop: '20px' }}>
          Naša kuhinja spaja tradicionalne recepte sa modernim tehnikama pripreme.
          Bilo da ste ljubitelj klasičnih mesnih jela, sveže pripremljenih pasta ili
          vegetarijanskih specijaliteta — naš meni ima nešto za svakoga. Posebno smo ponosni
          na naše sezonske specijalitete koji se menjaju kako bismo uvek ponudili
          najsvežije i najukusnije kombinacije.
        </p>
        <p style={{ color: '#2c2c2c', lineHeight: '1.9', fontSize: '1.05rem', marginTop: '20px' }}>
          Više od hrane, nudimo iskustvo. Topla i prijatna atmosfera, ljubazno osoblje i
          pažljivo osmišljen ambijent čine svaku posetu nezaboravnom. Bez obzira da li dolazite
          na poslovni ručak, porodičnu večeru ili romantičan izlazak — naš restoran je pravo
          mesto za vas.{' '}
          <span onClick={handleReservationClick} style={{
            color: '#c9a84c',
            textDecoration: 'underline',
            cursor: 'pointer',
            fontWeight: 'bold'
          }}>
            Rezervišite svoj sto
          </span>
          {' '}danas i uverite se sami zašto nas naši gosti uvek iznova biraju.
        </p>
      </div>

      {/* Info kartice */}
      <div className="row g-4 mb-4">
        {[
          { icon: '🕐', title: 'Radno vreme', lines: ['Pon - Pet: 08:00 - 23:00', 'Sub - Ned: 10:00 - 00:00'] },
          { icon: '📍', title: 'Lokacija', lines: ['Ulica bb, Grad', 'Srbija'] },
          { icon: '📞', title: 'Kontakt', lines: ['+381 xx xxx xxxx', 'restoran@email.com'] }
        ].map((card, i) => (
          <div key={i} className="col-md-4">
            <div style={{
              backgroundColor: '#1a1a1a',
              borderRadius: '12px',
              padding: '36px',
              textAlign: 'center',
              border: '1px solid #c9a84c',
              height: '100%'
            }}>
              <div style={{ fontSize: '2.5rem' }}>{card.icon}</div>
              <h4 style={{ color: '#c9a84c', margin: '16px 0 12px' }}>{card.title}</h4>
              {card.lines.map((line, j) => (
                <p key={j} style={{ color: '#ede8dc', margin: '4px 0' }}>{line}</p>
              ))}
            </div>
          </div>
        ))}
      </div>

      {/* Zašto mi */}
      <div style={{
        backgroundColor: '#fff9f0',
        border: '1px solid #e8c96d',
        borderRadius: '12px',
        padding: '48px',
        marginBottom: '40px',
        textAlign: 'center'
      }}>
        <h2 style={{ color: '#1a1a1a', marginBottom: '36px', borderBottom: '2px solid #c9a84c', paddingBottom: '16px' }}>
          Zašto izabrati nas?
        </h2>
        <div className="row g-3">
          {[
            { icon: '🥗', text: 'Sveži sastojci' },
            { icon: '👨‍🍳', text: 'Iskusni kuvari' },
            { icon: '🚀', text: 'Brza usluga' },
            { icon: '💯', text: 'Kvalitet garantovan' }
          ].map((item, i) => (
            <div key={i} className="col-md-3">
              <div style={{ fontSize: '2.5rem' }}>{item.icon}</div>
              <h6 style={{ color: '#2c2c2c', marginTop: '12px', fontWeight: 'bold' }}>{item.text}</h6>
            </div>
          ))}
        </div>
      </div>

      {/* CTA */}
      <div style={{
        background: 'linear-gradient(135deg, #1a1a1a 0%, #2c2c2c 100%)',
        borderRadius: '12px',
        padding: '48px',
        textAlign: 'center',
        marginBottom: '40px',
        border: '2px solid #c9a84c'
      }}>
        <h3 style={{ color: '#f5f0e8', marginBottom: '8px' }}>Već imate nalog?</h3>
        <p style={{ color: '#c9a84c', marginBottom: '24px' }}>Prijavite se i naručite već danas!</p>
        <button onClick={() => navigate('/login')} style={{
          backgroundColor: '#c9a84c',
          color: '#1a1a1a',
          border: 'none',
          padding: '14px 48px',
          borderRadius: '6px',
          fontSize: '1rem',
          fontWeight: 'bold',
          cursor: 'pointer'
        }}>
          Prijavite se
        </button>
      </div>
    </div>
  )
}

export default HomePage