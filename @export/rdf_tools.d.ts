// export R# package module type define for javascript/typescript language
//
//    imports "rdf_tools" from "pubchem";
//
// ref=pubchem.Rscript@pubchem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace rdf_tools {
   /**
     * @param lazy default value Is ``false``.
     * @param env default value Is ``null``.
   */
   function get_cid_tupleData(file: any, lazy?: boolean, env?: object): object;
   /**
     * @param lazy default value Is ``false``.
     * @param env default value Is ``null``.
   */
   function get_hash_tupleData(file: any, lazy?: boolean, env?: object): object;
   /**
     * @param typeof default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function to_ttlobject(ttl: any, typeof?: any, env?: object): object;
}
