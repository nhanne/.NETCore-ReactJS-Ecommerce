import React from 'react'
import { Link } from 'react-router-dom'
import { useState, useEffect, useContext } from 'react'
import { FilterContext } from '../store.Context'

function Categories() {
    const context = useContext(FilterContext)
    const [categories, setCategories] = useState([]);
   
    useEffect(() => {
        fetch(`/Home/getCategories`)
            .then(res => res.json())
            .then(response => {
                setCategories(response.categories)
            })
    }, [])

    return (
        <React.Fragment>
            <div className="grid__column-2">
                <nav className="category">
                    <ul className='category-list' id='mySidebar'>
                        <li key={0} className='category-item'>
                            <Link to="/Home/Store"
                                className='category-item__link'
                                onClick={() => context.setCategory('')}
                            >
                                New Stuff
                            </Link>
                        </li>
                        {categories.map(category => (
                            <li key={category.id} className='category-item'>
                                <Link to="/Home/Store"
                                    className='category-item__link'
                                    onClick={() => context.setCategory(category.name)}
                                >
                                    {category.name}
                                </Link>
                            </li>
                        ))}
                    </ul>
                </nav>
            </div>
            <button type="button" className="openbtn" onClick={() => openNav()}>☰ Danh mục</button>
        </React.Fragment>
    )
}
export default React.memo(Categories)