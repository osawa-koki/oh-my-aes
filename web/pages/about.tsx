import Layout from "../components/Layout";

export default function HelloWorld() {
  return (
    <Layout>
      <div id="About" className="mt-3">
        <h1>What is AES???</h1>
        <p className="mt-3">
          AESとは、'Advanced Encryption Standard'の略で、共通鍵暗号の一種です。<br />
          <br />
          暗号化と復号化に同じ鍵を使用します。<br />
          2023年の時点で、共通鍵暗号化アルゴリズムの中で最も広く使われているアルゴリズムです。<br />
          また、暗号化と復号化に同じ鍵を使用するため、鍵の管理が重要になります。<br />
          一般的には、鍵を暗号化するために、公開鍵暗号を使用します。<br />
          暗号化するためのキーを共有するために、別の暗号方式である公開鍵暗号を使用するということです。<br />
          なんだか、複雑ですね。<br />
          <br />
          暗号化対象はバイナリですが、このツールでは簡単のため、文字列のみを対象としています。<br />
          また、文字コードとしてUTF-8を使用しています。<br />
          <br />
          暗号化されたデータに関しては文字列で表現できない値も含まれるため、Base64エンコードを行っています。<br />
        </p>
      </div>
    </Layout>
  );
};
