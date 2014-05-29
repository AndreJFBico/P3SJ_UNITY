Shader "GLSL normal mapping" {
   Properties {
      _MainTex ("Texture Image", 2D) = "white" {} 
      _BumpMap ("Normal Map", 2D) = "bump" {}
      _Color ("Diffuse Material Color", Color) = (1,1,1,1) 
      _SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
      _Shininess ("Shininess", Float) = 10
   }
   SubShader {
      Pass {      
         Tags { "LightMode" = "ForwardBase" } 
            // pass for ambient light and first light source
 
         GLSLPROGRAM
 
         // User-specified properties
         uniform sampler2D _MainTex;
         uniform sampler2D _BumpMap;   
         uniform vec4 _BumpMap_ST;
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _Shininess;
 
         uniform vec3 _WorldSpaceCameraPos; 
         uniform mat4 _Object2World; // model matrix
         uniform mat4 _World2Object; // inverse model matrix
         uniform vec4 _WorldSpaceLightPos0; 
         uniform vec4 _LightColor0; 
 
         varying vec3 tangN;
         varying vec3 tangT;
         varying vec3 tangB;
         varying mat3 TBN;

         varying vec3 ex_Normal;
         varying vec2 ex_Texcoord;
         varying vec4 ex_Vertex;

         varying vec4 world_pos;
         varying vec4 tex_cor;
 
         #ifdef VERTEX
 
         attribute vec4 Tangent;
 
         void main()
         {                           
            ex_Normal = normalize(gl_NormalMatrix * vec3(gl_Normal));
            ex_Texcoord = vec2(gl_MultiTexCoord0.x, 1.0- gl_MultiTexCoord0.y);
            tex_cor = gl_MultiTexCoord0;
            ex_Vertex = (gl_ModelViewMatrix * gl_Vertex);
            world_pos = (_Object2World * gl_Vertex);
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

            /*tangT = normalize(vec3(
               _Object2World * vec4(vec3(Tangent), 0.0)));*/ //tangT
            tangT = normalize(vec3(
               gl_NormalMatrix * vec3(Tangent))); //tangT
            TBN[0] = tangT;

            tangN = normalize(vec3(
               vec4(gl_Normal, 0.0) * _World2Object)); //tangN
            TBN[2] = tangN;

            tangB = normalize(
               cross(tangN, tangT) 
               * Tangent.w); //tangB
            TBN[1] = tangB;  
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {

            //lookup normal obtained from normal map
            //vec3 N = normalize(vec3(2.0 * texture(_BumpMap, tex_cor) - 1.0));

            vec4 encodedNormal = texture2D(_BumpMap, _BumpMap_ST.xy * tex_cor.xy + _BumpMap_ST.zw);
            vec3 localCoords = vec3(2.0 * encodedNormal.ag - vec2(1.0), 0.0); // vec3 N

            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));

            vec3 N = normalize(TBN * localCoords);
            vec3 L;
            float attenuation; 
            if (0.0 == _WorldSpaceLightPos0.w)
            {
               attenuation = 1.0; // no attenuation
               L = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - world_pos);

               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance;

               L = normalize(vertexToLightSource);
            }

            float Ldist = length(L);
               //Aplying TBN matrix to L vector
            vec3 v;
           // v.x = dot(L, tangT);
           // v.y = dot(L, tangB);
           // v.z = dot(L, tangN);
            //L = normalize(v);
            /*------------------------*/

            vec3 E = normalize(-world_pos);//eyespace a posição da camera é 0,0,0
            vec3 H = normalize(L + E);
            vec3 R = reflect(-L,N);//eyespace a posição da camera é 0,0,0
            
            //Aplying TBN matrix to the half vector
            //v.x = dot(H, tangT);
           // v.y = dot(H, tangB);
           // v.z = dot(H, tangN);
            //H = normalize(v);

            vec3 ambient = vec3(gl_LightModel.ambient) * vec3(_Color);

            float NdotL = clamp(dot(N,L),0.0,1.0);
            vec3 diffuse = vec3(_LightColor0) * vec3(_Color) * NdotL;

            vec3 specular = vec3(0.0);
            if(NdotL > 0.0) {
               float NdotH = clamp(dot(R,H),0.0,1.0);
               float Blinn = pow(NdotH, _Shininess);
               specular = vec3(_LightColor0) * vec3(_SpecColor) * Blinn;
            }

            //attenuation = 1 / (1.0 +LightAttenuation.x * Ldist + LightAttenuation.y * pow(Ldist,2));

            gl_FragColor = texture2D(_MainTex, vec2(tex_cor)) * vec4(ambient 
               + (diffuse + specular) * attenuation, 1.0);
         }
 
         #endif
 
         ENDGLSL
      }
 
      Pass {      
         Tags { "LightMode" = "ForwardAdd" } 
            // pass for additional light sources
         Blend One One // additive blending 
 
         GLSLPROGRAM
 
         // User-specified properties
         uniform sampler2D _MainTex;
         uniform sampler2D _BumpMap;   
         uniform vec4 _BumpMap_ST;
         uniform vec4 _Color; 
         uniform vec4 _SpecColor; 
         uniform float _Shininess;
 
         uniform vec3 _WorldSpaceCameraPos; 
         uniform mat4 _Object2World; // model matrix
         uniform mat4 _World2Object; // inverse model matrix
         uniform vec4 _WorldSpaceLightPos0; 
         uniform vec4 _LightColor0; 
 
         varying vec3 tangN;
         varying vec3 tangT;
         varying vec3 tangB;
         varying mat3 TBN;

         varying vec3 ex_Normal;
         varying vec2 ex_Texcoord;
         varying vec4 ex_Vertex;

         varying vec4 world_pos;
         varying vec4 tex_cor;
 
         #ifdef VERTEX
 
         attribute vec4 Tangent;
 
         void main()
         {                           
            ex_Normal = normalize(gl_NormalMatrix * vec3(gl_Normal));
            ex_Texcoord = vec2(gl_MultiTexCoord0.x, 1.0- gl_MultiTexCoord0.y);
            tex_cor = gl_MultiTexCoord0;
            ex_Vertex = (gl_ModelViewMatrix * gl_Vertex);
            world_pos = (_Object2World * gl_Vertex);
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;

            /*tangT = normalize(vec3(
               _Object2World * vec4(vec3(Tangent), 0.0)));*/ //tangT
            tangT = normalize(vec3(
               gl_NormalMatrix * vec3(Tangent))); //tangT
            TBN[0] = tangT;

            tangN = normalize(vec3(
               vec4(gl_Normal, 0.0) * _World2Object)); //tangN
            TBN[2] = tangN;

            tangB = normalize(
               cross(tangN, tangT) 
               * Tangent.w); //tangB
            TBN[1] = tangB;  
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main()
         {

            //lookup normal obtained from normal map
            //vec3 N = normalize(vec3(2.0 * texture(_BumpMap, tex_cor) - 1.0));

            vec4 encodedNormal = texture2D(_BumpMap, _BumpMap_ST.xy * tex_cor.xy + _BumpMap_ST.zw);
            vec3 localCoords = vec3(2.0 * encodedNormal.ag - vec2(1.0), 0.0); // vec3 N

            localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));

            vec3 N = normalize(TBN * localCoords);
            vec3 L;
            float attenuation; 
            if (0.0 == _WorldSpaceLightPos0.w)
            {
               attenuation = 1.0; // no attenuation
               L = normalize(vec3(_WorldSpaceLightPos0));
            } 
            else // point or spot light
            {
               vec3 vertexToLightSource = 
                  vec3(_WorldSpaceLightPos0 - world_pos);

               float distance = length(vertexToLightSource);
               attenuation = 1.0 / distance;

               L = normalize(vertexToLightSource);
            }

            float Ldist = length(L);
               //Aplying TBN matrix to L vector
            vec3 v;
           // v.x = dot(L, tangT);
           // v.y = dot(L, tangB);
           // v.z = dot(L, tangN);
            //L = normalize(v);
            /*------------------------*/

            vec3 E = normalize(-world_pos);//eyespace a posição da camera é 0,0,0
            vec3 H = normalize(L + E);
            vec3 R = reflect(-L,N);//eyespace a posição da camera é 0,0,0
            
            //Aplying TBN matrix to the half vector
            //v.x = dot(H, tangT);
           // v.y = dot(H, tangB);
           // v.z = dot(H, tangN);
            //H = normalize(v);

            vec3 ambient = vec3(gl_LightModel.ambient) * vec3(_Color);

            float NdotL = clamp(dot(N,L),0.0,1.0);
            vec3 diffuse = vec3(_LightColor0) * vec3(_Color) * NdotL;

            vec3 specular = vec3(0.0);
            if(NdotL > 0.0) {
               float NdotH = clamp(dot(R,H),0.0,1.0);
               float Blinn = pow(NdotH, _Shininess);
               specular = vec3(_LightColor0) * vec3(_SpecColor) * Blinn;
            }

            //attenuation = 1 / (1.0 +LightAttenuation.x * Ldist + LightAttenuation.y * pow(Ldist,2));

            gl_FragColor = texture2D(_MainTex, vec2(tex_cor)) * vec4((diffuse + specular) * attenuation, 1.0) / 4;
         }
 
         #endif
 
         ENDGLSL
      }
   } 
   // The definition of a fallback shader should be commented out 
   // during development:
   // Fallback "Bumped Specular"
}