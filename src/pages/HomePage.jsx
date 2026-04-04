import { Link, useNavigate } from 'react-router-dom'
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
      <div className="p-5 mb-4 bg-dark text-white rounded-3 text-center">
        <h1 className="display-4 fw-bold">🍽️ Dobrodošli u naš Restoran</h1>
        <p className="lead mt-3">
          Uživajte u autentičnim okusima domaće kuhinje. 
          Sveži sastojci, ljubav u svakom zalogaju.
        </p>
        <div className="mt-4">
          <Link to="/menu" className="btn btn-light btn-lg me-3">Pogledaj Meni</Link>
          <Link to="/register" className="btn btn-outline-light btn-lg">Registruj se</Link>
        </div>
      </div>

      {/* Opis restorana */}
      <div className="card p-5 mb-4">
        <h2 className="text-center mb-4">O našem restoranu</h2>
        <p className="lead text-muted">
            Naš restoran osnovan je sa jednom misijom — donijeti autentičan ukus domaće kuhinje 
            na vaš tanjir. Svaki dan naši kuvari sa godinama iskustva pripremaju jela od najsvježijih 
            lokalnih namirnica, birajući samo ono najbolje što nam sezona nudi. Kod nas nema kompromisa 
            kada je kvalitet u pitanju.
        </p>
        <p className="text-muted mt-3">
            Naša kuhinja spaja tradicionalne recepte sa modernim tehnikama pripreme. 
            Bilo da ste ljubitelj klasičnih mesnih jela, svježe pripremljenih pasta ili 
            vegetarijanskih specijaliteta — naš meni ima nešto za svakoga. Posebno smo ponosni 
            na naše sezonske specijalitete koji se mijenjaju kako bismo uvijek ponudili 
            najsvježije i najukusnije kombinacije.
        </p>
        <p className="text-muted mt-3">
            Više od hrane, nudimo iskustvo. Topla i prijatna atmosfera, ljubazno osoblje i 
            pažljivo osmišljen ambijent čine svaki posjet nezaboravnim. Bez obzira da li dolazite 
            na poslovni ručak, porodičnu večeru ili romantičan izlazak — naš restoran je pravo 
            mesto za vas.  
            mjesto za vas.{' '}
            <span onClick={handleReservationClick}
                style={{ cursor: 'pointer', color: '#0d6efd', textDecoration: 'underline' }}>
                Rezervišite svoj sto
            </span>
            {' '}danas i uvjerite se sami zašto nas naši gosti uvijek iznova biraju.
        </p>
      </div>

      {/* Info kartice */}
      <div className="row g-4 mb-4">
        <div className="col-md-4">
          <div className="card h-100 text-center p-4">
            <div style={{ fontSize: '3rem' }}>🕐</div>
            <h4 className="mt-3">Radno vrijeme</h4>
            <p className="text-muted">Pon - Pet: 08:00 - 23:00</p>
            <p className="text-muted">Sub - Ned: 10:00 - 00:00</p>
          </div>
        </div>
        <div className="col-md-4">
          <div className="card h-100 text-center p-4">
            <div style={{ fontSize: '3rem' }}>📍</div>
            <h4 className="mt-3">Lokacija</h4>
            <p className="text-muted">Ulica bb, Grad</p>
            <p className="text-muted">Srbija</p>
          </div>
        </div>
        <div className="col-md-4">
          <div className="card h-100 text-center p-4">
            <div style={{ fontSize: '3rem' }}>📞</div>
            <h4 className="mt-3">Kontakt</h4>
            <p className="text-muted">+381 xx xxx xxxx</p>
            <p className="text-muted">restoran@email.com</p>
          </div>
        </div>
      </div>

      {/* Zašto mi */}
      <div className="card p-5 text-center mb-4">
        <h2 className="mb-4">Zašto izabrati nas?</h2>
        <div className="row g-3">
          <div className="col-md-3">
            <div style={{ fontSize: '2rem' }}>🥗</div>
            <h6 className="mt-2">Sveži sastojci</h6>
          </div>
          <div className="col-md-3">
            <div style={{ fontSize: '2rem' }}>👨‍🍳</div>
            <h6 className="mt-2">Iskusni kuvari</h6>
          </div>
          <div className="col-md-3">
            <div style={{ fontSize: '2rem' }}>🚀</div>
            <h6 className="mt-2">Brza dostava</h6>
          </div>
          <div className="col-md-3">
            <div style={{ fontSize: '2rem' }}>💯</div>
            <h6 className="mt-2">Kvalitet garantovan</h6>
          </div>
        </div>
      </div>

      {/* CTA - poziv na akciju */}
      <div className="text-center mb-4">
        <h3>Već imate nalog?</h3>
        <Link to="/login" className="btn btn-dark btn-lg mt-2">Prijavite se</Link>
      </div>
    </div>
  )
}

export default HomePage