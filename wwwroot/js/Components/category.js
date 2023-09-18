import React from 'react' // nạp thư viện react
import { useState, useEffect } from 'react'
export default function Categories({ setCateName }) {
    const [categories, setCategories] = useState([]);
    const handleCateChange = (event, categoryName) => {
        setCateName(categoryName);
    };

    useEffect(() => {
        fetch(`/Home/getCategories`)
            .then(res => res.json())
            .then(reponse => {
                setCategories(reponse.categories)
            })
    }, [])

    return (
        <React.Fragment>
            <div className="grid__column-2">
                <nav className="category">
                    <ul className='category-list' id='mySidebar'>
                        <li key={0} className='category-item'>
                            <a
                                className='category-item__link'
                                onClick={(event) => handleCateChange(event, ' ')}>
                                New Stuff
                            </a>
                        </li>
                        {categories.map(category => (
                            <li key={category.id} className='category-item'>
                                <a
                                    className='category-item__link'
                                    onClick={(event) => handleCateChange(event, category.name)}>
                                    {category.name}
                                </a>
                            </li>
                        ))}
                    </ul>
                </nav>
            </div>
            <button className="openbtn" onClick={() => openNav()}>☰ Danh mục</button>
        </React.Fragment>
    )
}
