import React from 'react'
import { useState, useEffect } from 'react'

export default function Pagination(props) {


    return (
        <div id="pagination" className="MenuTrang pagination-container">
            <ul className="pagination">
                {props.totalPages > 1 ? (
                    <>
                        {Array.from({ length: props.totalPages }, (_, i) => (
                            <li className={`page-item`} key={i}>
                                <a
                                    onClick={() => props.setPage(i + 1)}
                                    className={`page-link ${i + 1 === props.page ? 'active' : ''}`}
                                >
                                    {i + 1}
                                </a>
                            </li>
                        ))}
                    </>
                ) : (
                    ''
                )}
            </ul>
        </div>
    )
}
