import { useEffect, useState } from "react";

export default function Home() {

  const [orders, setOrders] = useState([])
  const [message, setMessage] = useState<string>('')

  useEffect(() => {
    getOrders();
  }, [])

  function getOrders() {
    fetch('http://localhost:8002/orders', {
      method: 'GET'
    }).then(res => res.json())
      .then(data => setOrders(data))
  }

  function postOrder(userId: number) {
    fetch(`http://localhost:8002/orders?userId=${userId}`, {
      method: 'POST'
    })
  }

  return (
    <div className="flex flex-col">
      <p className="font-bold">All Orders</p>
      {orders && orders.length > 0 ?
        <>
          {orders.map(order => (
            <>
              <a href={`/orders/${order['orderId']}`}>Order for user #{order['userId']}</a>
              <br />
            </>
          ))}
        </>
        :
        <p>There are no orders yet.</p>
      }
      <br />
      <button className="border-2" onClick={() => postOrder(1)}>Place Order for user #1 (100 Credit)</button>
      <button className="border-2" onClick={() => postOrder(2)}>Place Order for user #2 (99 Credit)</button>
      <p>{message}</p>
    </div>
  );
}
